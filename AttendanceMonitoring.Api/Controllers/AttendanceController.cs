using System.Security.Claims;
using AttendanceMonitoring.Application.Attendance.DTOs;
using AttendanceMonitoring.Application.Attendance.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [FromQuery] string? studentName,
        CancellationToken ct)
    {
        _logger.LogInformation("Запрос списка записей. Фильтр: тип={Type}, студент={StudentName}", type, studentName);
        var filter = new AttendanceFilter(type, from, to, userId, studentName);
        var result = await _service.GetAsync(filter, ct);
        _logger.LogInformation("Возвращено записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceRequest request,
        CancellationToken ct)
    {
        // Берём userId из JWT токена автоматически
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = int.Parse(userIdClaim ?? "1");

        _logger.LogInformation("Создание записи: студент={StudentName}, тип={Type}", request.StudentName, request.Type);
        var result = await _service.CreateAsync(request, userId, ct);
        _logger.LogInformation("Запись создана с Id={Id}", result.Id);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize]
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