using AttendanceMonitoring.Application.Attendance.DTOs;

namespace AttendanceMonitoring.Application.Attendance.Interfaces;

public interface IAnalyticsService
{
    Task<AttendanceAnalyticsDto> GetAnalyticsAsync(CancellationToken ct);
    Task<byte[]> ExportCsvAsync(CancellationToken ct);
}