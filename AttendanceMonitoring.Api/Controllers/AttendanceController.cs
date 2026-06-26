using AttendanceMonitoring.Application.Attendance.DTOs;
using AttendanceMonitoring.Application.Attendance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceMonitoring.Api.Controllers;

[ApiController]
[Route("api/attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(IAttendanceService service, ILogger<AttendanceController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? type,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int? userId,
        CancellationToken ct)
    {
        _logger.LogInformation("Запрос списка записей. Фильтр: тип={Type}, с={From}, по={To}", type, from, to);
        var filter = new AttendanceFilter(type, from, to, userId);
        var result = await _service.GetAsync(filter, ct);
        _logger.LogInformation("Возвращено записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Создание записи: тип={Type}, источник={Source}", request.Type, request.Source);
        var result = await _service.CreateAsync(request, userId: 1, ct);
        _logger.LogInformation("Запись создана с Id={Id}", result.Id);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        _logger.LogInformation("Удаление записи Id={Id}", id);
        var deleted = await _service.DeleteAsync(id, ct);
        if (!deleted)
        {
            _logger.LogWarning("Запись Id={Id} не найдена", id);
            return NotFound();
        }
        _logger.LogInformation("Запись Id={Id} удалена", id);
        return NoContent();
    }
}