using AttendanceMonitoring.Application.Attendance.DTOs;

namespace AttendanceMonitoring.Application.Attendance.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
}