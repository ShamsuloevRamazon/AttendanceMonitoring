namespace AttendanceMonitoring.Domain.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    // Имя студента о котором запись
    public string StudentName { get; set; } = string.Empty;

    // Тип: "Посещение", "Пропуск", "Опоздание", "Отметка входа"
    public string Type { get; set; } = string.Empty;

    // Источник: "Турникет", "Преподаватель", "Ручной ввод"
    public string Source { get; set; } = string.Empty;

    // Группа: "Группа ИС-21" и т.д.
    public string Description { get; set; } = string.Empty;

    // Важность: "Info", "Warning", "Critical"
    public string Severity { get; set; } = "Info";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}