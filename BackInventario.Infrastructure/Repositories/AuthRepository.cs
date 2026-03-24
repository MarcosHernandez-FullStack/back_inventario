using System.Data;
using BackInventario.Application.DTOs.Auth;
using BackInventario.Application.Interfaces;
using BackInventario.Infrastructure.Data;
using Dapper;

namespace BackInventario.Infrastructure.Repositories;

public class AuthRepository(DbConnectionFactory db) : IAuthRepository
{
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<LoginResponseDto>(
            "sp_LoginUsuario",
            new { dto.Correo, dto.Contrasena },
            commandType: CommandType.StoredProcedure
        );
    }
}
