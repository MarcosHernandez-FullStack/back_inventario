using System.Data;
using BackInventario.Application.DTOs.Usuario;
using BackInventario.Application.Interfaces;
using BackInventario.Infrastructure.Data;
using Dapper;

namespace BackInventario.Infrastructure.Repositories;

public class UsuarioRepository(DbConnectionFactory db) : IUsuarioRepository
{
    public async Task<IEnumerable<UsuarioDto>> ListarAsync()
    {
        using var conn = db.CreateConnection();
        return await conn.QueryAsync<UsuarioDto>(
            "sp_ListarUsuarios", commandType: CommandType.StoredProcedure);
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<UsuarioDto>(
            "sp_ObtenerUsuario", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(CrearUsuarioDto dto)
    {
        using var conn = db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "sp_CrearUsuario", dto, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ActualizarEstadoAsync(int id, string estado, string actualizadoPor)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_ActualizarEstadoUsuario",
            new { Id = id, Estado = estado, ActualizadoPor = actualizadoPor },
            commandType: CommandType.StoredProcedure);
        return rows > 0;
    }
}
