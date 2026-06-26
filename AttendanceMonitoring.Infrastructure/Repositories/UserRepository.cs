using AttendanceMonitoring.Application.Attendance.Interfaces;
using AttendanceMonitoring.Domain.Entities;
using AttendanceMonitoring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoring.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AppUser?> GetByUserNameAsync(string userName, CancellationToken ct)
    {
        return await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserName == userName, ct);
    }
}