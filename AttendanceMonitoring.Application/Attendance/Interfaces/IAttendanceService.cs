using AttendanceMonitoring.Application.Attendance.DTOs;

namespace AttendanceMonitoring.Application.Attendance.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceRecordDto> CreateAsync(CreateAttendanceRequest request, int userId, CancellationToken ct);
    Task<IReadOnlyList<AttendanceRecordDto>> GetAsync(AttendanceFilter filter, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}