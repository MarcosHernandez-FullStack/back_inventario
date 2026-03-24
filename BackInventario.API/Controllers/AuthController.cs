using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackInventario.Application.DTOs.Auth;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace BackInventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService, IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await authService.LoginAsync(dto);
        if (result is null)
            return Unauthorized(new { mensaje = "Credenciales incorrectas." });

        result.Token = GenerarToken(result);
        return Ok(result);
    }

    private string GenerarToken(LoginResponseDto user)
    {
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(double.Parse(config["Jwt:ExpireHours"] ?? "8"));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   user.Correo),
            new Claim(JwtRegisteredClaimNames.Email, user.Correo),
            new Claim(ClaimTypes.NameIdentifier,     user.Id.ToString()),
            new Claim("role",                        user.Rol),
            new Claim("nombres",                     user.Nombres),
        };

        var token = new JwtSecurityToken(
            issuer:             config["Jwt:Issuer"],
            audience:           config["Jwt:Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
