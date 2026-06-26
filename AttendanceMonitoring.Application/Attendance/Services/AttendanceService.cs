using AttendanceMonitoring.Application.Attendance.DTOs;
using AttendanceMonitoring.Application.Attendance.Interfaces;
using AttendanceMonitoring.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceMonitoring.Application.Attendance.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repo;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "attendance_all";

    public AttendanceService(IAttendanceRepository repo, IMemoryCache cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<AttendanceRecordDto> CreateAsync(CreateAttendanceRequest request, int userId, CancellationToken ct)
    {
        var record = new AttendanceRecord
        {
            UserId = userId,
            Type = request.Type,
            Source = request.Source,
            Description = request.Description,
            Severity = request.Severity,
            CreatedAt = DateTime.UtcNow
        };

        var saved = await _repo.AddAsync(record, ct);

        // Сбрасываем кэш при добавлении новой записи
        _cache.Remove(CacheKey);

        return new AttendanceRecordDto(
            saved.Id,
            saved.UserId,
            saved.User?.UserName ?? "Неизвестно",
            saved.Type,
            saved.Source,
            saved.Description,
            saved.Severity,
            saved.CreatedAt
        );
    }

    public async Task<IReadOnlyList<AttendanceRecordDto>> GetAsync(AttendanceFilter filter, CancellationToken ct)
    {
        // Берём все записи из кэша или из базы
        if (!_cache.TryGetValue(CacheKey, out IReadOnlyList<AttendanceRecord>? all))
        {
            all = await _repo.GetAllAsync(ct);

            _cache.Set(CacheKey, all, TimeSpan.FromMinutes(5));
        }

        var filtered = all!.AsEnumerable();

        if (!string.IsNullOrEmpty(filter.Type))
            filtered = filtered.Where(r => r.Type == filter.Type);

        if (filter.From.HasValue)
            filtered = filtered.Where(r => r.CreatedAt >= filter.From.Value);

        if (filter.To.HasValue)
            filtered = filtered.Where(r => r.CreatedAt <= filter.To.Value);

        if (filter.UserId.HasValue)
            filtered = filtered.Where(r => r.UserId == filter.UserId.Value);

        return filtered.Select(r => new AttendanceRecordDto(
            r.Id,
            r.UserId,
            r.User?.UserName ?? "Неизвестно",
            r.Type,
            r.Source,
            r.Description,
            r.Severity,
            r.CreatedAt
        )).ToList();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var result = await _repo.DeleteAsync(id, ct);

        // Сбрасываем кэш при удалении
        if (result) _cache.Remove(CacheKey);

        return result;
    }
}