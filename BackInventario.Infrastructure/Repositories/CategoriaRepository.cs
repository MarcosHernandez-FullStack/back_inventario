using System.Data;
using BackInventario.Application.DTOs.Categoria;
using BackInventario.Application.Interfaces;
using BackInventario.Infrastructure.Data;
using Dapper;

namespace BackInventario.Infrastructure.Repositories;

public class CategoriaRepository(DbConnectionFactory db) : ICategoriaRepository
{
    public async Task<IEnumerable<CategoriaDto>> ListarAsync()
    {
        using var conn = db.CreateConnection();
        return await conn.QueryAsync<CategoriaDto>(
            "sp_ListarCategorias", commandType: CommandType.StoredProcedure);
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<CategoriaDto>(
            "sp_ObtenerCategoria", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(CrearCategoriaDto dto)
    {
        using var conn = db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "sp_CrearCategoria", dto, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ActualizarAsync(int id, string nombre, string actualizadoPor)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_ActualizarCategoria",
            new { Id = id, Nombre = nombre, ActualizadoPor = actualizadoPor },
            commandType: CommandType.StoredProcedure);
        return rows > 0;
    }

    public async Task<bool> CambiarEstadoAsync(int id, string estado, string actualizadoPor)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_CambiarEstadoCategoria",
            new { Id = id, Estado = estado, ActualizadoPor = actualizadoPor },
            commandType: CommandType.StoredProcedure);
        return rows > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_EliminarCategoria", new { Id = id }, commandType: CommandType.StoredProcedure);
        return rows > 0;
    }
}
