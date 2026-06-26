namespace AttendanceMonitoring.Domain.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    // Тип: "Посещение", "Пропуск", "Опоздание", "Отметка входа"
    public string Type { get; set; } = string.Empty;

    // Источник: "Турникет", "Преподаватель", "Ручной ввод"
    public string Source { get; set; } = string.Empty;

    // Описание: "Группа ИС-21", "Пара по математике" и т.д.
    public string Description { get; set; } = string.Empty;

    // Важность: "Info", "Warning" (опоздание), "Critical" (прогул)
    public string Severity { get; set; } = "Info";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}