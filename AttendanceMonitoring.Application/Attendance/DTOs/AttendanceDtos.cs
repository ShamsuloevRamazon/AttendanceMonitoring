namespace AttendanceMonitoring.Application.Attendance.DTOs;

// Входящий запрос (от клиента)
public record CreateAttendanceRequest(
    string Type,
    string Source,
    string Description,
    string Severity
);

// Фильтр для поиска записей
public record AttendanceFilter(
    string? Type,
    DateTime? From,
    DateTime? To,
    int? UserId
);

// Ответ клиенту
public record AttendanceRecordDto(
    int Id,
    int UserId,
    string UserName,
    string Type,
    string Source,
    string Description,
    string Severity,
    DateTime CreatedAt
);
// Запрос на вход
public record LoginRequest(
    string UserName,
    string Password
);

// Ответ с токеном
public record LoginResponse(
    string Token,
    string UserName,
    string Role
);
// Посещаемость по дням
public record AttendanceByDayDto(
    string Date,
    int Total,
    int Visits,
    int Absences,
    int Lates
);

// Активные группы
public record GroupActivityDto(
    string GroupName,
    int TotalRecords,
    int Absences
);

// Итоговая аналитика
public record AttendanceAnalyticsDto(
    int TotalRecords,
    int TotalVisits,
    int TotalAbsences,
    int TotalLates,
    List<AttendanceByDayDto> ByDay,
    List<GroupActivityDto> ActiveGroups
);