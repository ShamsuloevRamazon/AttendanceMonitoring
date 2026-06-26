using AttendanceMonitoring.Application.Attendance.DTOs;
using AttendanceMonitoring.Application.Attendance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceMonitoring.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Попытка входа пользователя: {UserName}", request.UserName);

        var result = await _authService.LoginAsync(request, ct);

        if (result is null)
        {
            _logger.LogWarning("Неудачная попытка входа: {UserName}", request.UserName);
            return Unauthorized("Неверный логин или пароль");
        }

        _logger.LogInformation("Успешный вход: {UserName}, роль: {Role}", result.UserName, result.Role);
        return Ok(result);
    }
}