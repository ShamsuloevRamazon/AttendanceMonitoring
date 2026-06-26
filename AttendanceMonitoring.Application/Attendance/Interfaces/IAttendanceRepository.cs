using AttendanceMonitoring.Domain.Entities;

namespace AttendanceMonitoring.Application.Attendance.Interfaces;

public interface IAttendanceRepository
{
    Task<AttendanceRecord> AddAsync(AttendanceRecord record, CancellationToken ct);
    Task<IReadOnlyList<AttendanceRecord>> GetAllAsync(CancellationToken ct);
    Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}