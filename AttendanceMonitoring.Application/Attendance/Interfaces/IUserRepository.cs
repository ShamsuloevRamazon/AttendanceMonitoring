using AttendanceMonitoring.Domain.Entities;

namespace AttendanceMonitoring.Application.Attendance.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByUserNameAsync(string userName, CancellationToken ct);
}