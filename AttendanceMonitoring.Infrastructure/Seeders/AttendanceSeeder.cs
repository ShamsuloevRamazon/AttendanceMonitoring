using AttendanceMonitoring.Domain.Entities;
using AttendanceMonitoring.Infrastructure.Persistence;

namespace AttendanceMonitoring.Infrastructure.Seeders;

public static class AttendanceSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Если записи уже есть — не добавляем
        if (db.AttendanceRecords.Any()) return;

        var random = new Random();

        var types = new[] { "Посещение", "Пропуск", "Опоздание", "Отметка входа" };
        var sources = new[] { "Турникет", "Преподаватель", "Ручной ввод", "Приложение" };
        var groups = new[] { "Группа ИС-21", "Группа ИС-22", "Группа ПР-11", "Группа ПР-12" };
        var severities = new[] { "Info", "Warning", "Critical" };

        var records = new List<AttendanceRecord>();

        // Генерируем 50 записей за последние 30 дней
        for (int i = 0; i < 50; i++)
        {
            var type = types[random.Next(types.Length)];

            // Опоздание = Warning, Пропуск = Critical, остальное = Info
            var severity = type switch
            {
                "Опоздание" => "Warning",
                "Пропуск" => "Critical",
                _ => "Info"
            };

            records.Add(new AttendanceRecord
            {
                UserId = 1,
                Type = type,
                Source = sources[random.Next(sources.Length)],
                Description = groups[random.Next(groups.Length)],
                Severity = severity,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
                                           .AddHours(-random.Next(0, 8))
            });
        }

        await db.AttendanceRecords.AddRangeAsync(records);
        await db.SaveChangesAsync();
    }
}