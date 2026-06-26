namespace AttendanceMonitoring.Application.Attendance.DTOs;

public record CreateAttendanceRequest(
    string StudentName,
    string Type,
    string Source,
    string Description,
    string Severity
);

public record AttendanceFilter(
    string? Type,
    DateTime? From,
    DateTime? To,
    int? UserId,
    string? StudentName
);

public record AttendanceRecordDto(
    int Id,
    int UserId,
    string UserName,
    string StudentName,
    string Type,
    string Source,
    string Description,
    string Severity,
    DateTime CreatedAt
);

public record LoginRequest(
    string UserName,
    string Password
);

public record LoginResponse(
    string Token,
    string UserName,
    string Role
);

public record AttendanceByDayDto(
    string Date,
    int Total,
    int Visits,
    int Absences,
    int Lates
);

public record GroupActivityDto(
    string GroupName,
    int TotalRecords,
    int Absences
);

public record AttendanceAnalyticsDto(
    int TotalRecords,
    int TotalVisits,
    int TotalAbsences,
    int TotalLates,
    List<AttendanceByDayDto> ByDay,
    List<GroupActivityDto> ActiveGroups
);