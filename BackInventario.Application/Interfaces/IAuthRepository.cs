using BackInventario.Application.DTOs.Auth;

namespace BackInventario.Application.Interfaces;

public interface IAuthRepository
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}
