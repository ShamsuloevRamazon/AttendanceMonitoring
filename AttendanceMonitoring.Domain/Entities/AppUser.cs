namespace AttendanceMonitoring.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public Role? Role { get; set; }
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
}