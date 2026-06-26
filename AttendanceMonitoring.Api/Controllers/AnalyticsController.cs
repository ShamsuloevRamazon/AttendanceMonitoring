using AttendanceMonitoring.Application.Attendance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceMonitoring.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analytics;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(IAnalyticsService analytics, ILogger<AnalyticsController> logger)
    {
        _analytics = analytics;
        _logger = logger;
    }

    // GET /api/analytics
    [HttpGet]
    public async Task<IActionResult> GetAnalytics(CancellationToken ct)
    {
        _logger.LogInformation("Запрос аналитики посещаемости");
        var result = await _analytics.GetAnalyticsAsync(ct);
        return Ok(result);
    }

    // GET /api/analytics/export
    [HttpGet("export")]
    public async Task<IActionResult> ExportCsv(CancellationToken ct)
    {
        _logger.LogInformation("Экспорт CSV-отчёта");
        var bytes = await _analytics.ExportCsvAsync(ct);
        return File(bytes, "text/csv", $"attendance_{DateTime.Now:yyyyMMdd}.csv");
    }
}