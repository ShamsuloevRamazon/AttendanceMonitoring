using AttendanceMonitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoring.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Analyst" },
            new Role { Id = 3, Name = "Operator" }
        );

        modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                UserName = "admin",
                PasswordHash = "admin123",
                RoleId = 1,
                IsActive = true
            },
            new AppUser
            {
                Id = 2,
                UserName = "analyst",
                PasswordHash = "analyst123",
                RoleId = 2,
                IsActive = true
            },
            new AppUser
            {
                Id = 3,
                UserName = "operator",
                PasswordHash = "operator123",
                RoleId = 3,
                IsActive = true
            }
        );
    }
}