using BackInventario.Application.DTOs.Auth;
using BackInventario.Application.Interfaces;

namespace BackInventario.Application.Services;

public class AuthService(IAuthRepository authRepository)
{
    public Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        => authRepository.LoginAsync(dto);
}
