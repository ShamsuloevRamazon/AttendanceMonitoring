using System.Text;
using AttendanceMonitoring.Application.Attendance.DTOs;
using AttendanceMonitoring.Application.Attendance.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceMonitoring.Application.Attendance.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAttendanceRepository _repo;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "analytics_result";

    public AnalyticsService(IAttendanceRepository repo, IMemoryCache cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<AttendanceAnalyticsDto> GetAnalyticsAsync(CancellationToken ct)
    {
        if (_cache.TryGetValue(CacheKey, out AttendanceAnalyticsDto? cached))
            return cached!;

        var all = await _repo.GetAllAsync(ct);

        var totalRecords = all.Count;
        var totalVisits = all.Count(r => r.Type == "Посещение");
        var totalAbsences = all.Count(r => r.Type == "Пропуск");
        var totalLates = all.Count(r => r.Type == "Опоздание");

        // Посещаемость по дням
        var byDay = all
            .GroupBy(r => r.CreatedAt.Date)
            .OrderBy(g => g.Key)
            .Select(g => new AttendanceByDayDto(
                g.Key.ToString("yyyy-MM-dd"),
                g.Count(),
                g.Count(r => r.Type == "Посещение"),
                g.Count(r => r.Type == "Пропуск"),
                g.Count(r => r.Type == "Опоздание")
            )).ToList();

        // Активные группы
        var activeGroups = all
            .GroupBy(r => r.Description)
            .OrderByDescending(g => g.Count())
            .Select(g => new GroupActivityDto(
                g.Key,
                g.Count(),
                g.Count(r => r.Type == "Пропуск")
            )).ToList();

        var result = new AttendanceAnalyticsDto(
            totalRecords,
            totalVisits,
            totalAbsences,
            totalLates,
            byDay,
            activeGroups
        );

        _cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));

        return result;
    }

    public async Task<byte[]> ExportCsvAsync(CancellationToken ct)
    {
        var all = await _repo.GetAllAsync(ct);

        var sb = new StringBuilder();

        // Заголовок CSV
        sb.AppendLine("Id,UserId,UserName,StudentName,Type,Source,Description,Severity,CreatedAt");

        // Данные
        foreach (var r in all)
        {
            sb.AppendLine($"{r.Id},{r.UserId},{r.User?.UserName ?? "Неизвестно"},{r.StudentName},{r.Type},{r.Source},{r.Description},{r.Severity},{r.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}