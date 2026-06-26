using AttendanceMonitoring.Application.Attendance.Interfaces;
using AttendanceMonitoring.Domain.Entities;
using AttendanceMonitoring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoring.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _db;

    public AttendanceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AttendanceRecord> AddAsync(AttendanceRecord record, CancellationToken ct)
    {
        _db.AttendanceRecords.Add(record);
        await _db.SaveChangesAsync(ct);

        // Подгружаем пользователя чтобы userName не был "Неизвестно"
        await _db.Entry(record).Reference(r => r.User).LoadAsync(ct);

        return record;
    }

    public async Task<IReadOnlyList<AttendanceRecord>> GetAllAsync(CancellationToken ct)
    {
        return await _db.AttendanceRecords
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _db.AttendanceRecords
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var record = await _db.AttendanceRecords.FindAsync(new object[] { id }, ct);
        if (record is null) return false;

        _db.AttendanceRecords.Remove(record);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}