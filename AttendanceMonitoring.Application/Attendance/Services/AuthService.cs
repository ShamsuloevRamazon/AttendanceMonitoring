using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AttendanceMonitoring.Application.Attendance.DTOs;
using AttendanceMonitoring.Application.Attendance.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceMonitoring.Application.Attendance.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await _userRepo.GetByUserNameAsync(request.UserName, ct);

        if (user is null || user.PasswordHash != request.Password)
            return null;

        var token = GenerateToken(user.Id, user.UserName, user.Role!.Name);

        return new LoginResponse(token, user.UserName, user.Role.Name);
    }

    private string GenerateToken(int userId, string userName, string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}