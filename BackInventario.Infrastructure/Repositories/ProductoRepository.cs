using System.Data;
using BackInventario.Application.DTOs.Producto;
using BackInventario.Application.Interfaces;
using BackInventario.Infrastructure.Data;
using Dapper;

namespace BackInventario.Infrastructure.Repositories;

public class ProductoRepository(DbConnectionFactory db) : IProductoRepository
{
    public async Task<IEnumerable<ProductoDto>> ListarAsync(string? nombre, int? idCategoria)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryAsync<ProductoDto>(
            "sp_ListarProductos",
            new { Nombre = nombre, IdCategoria = idCategoria },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<ProductoDto>(
            "sp_ObtenerProducto", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CrearAsync(CrearProductoDto dto)
    {
        using var conn = db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "sp_CrearProducto", dto, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarProductoDto dto)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_ActualizarProducto",
            new
            {
                Id             = id,
                dto.Nombre,
                dto.Descripcion,
                dto.Precio,
                dto.Cantidad,
                dto.IdCategoria,
                dto.ActualizadoPor,
            },
            commandType: CommandType.StoredProcedure);
        return rows > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_EliminarProducto", new { Id = id }, commandType: CommandType.StoredProcedure);
        return rows > 0;
    }

    public async Task<bool> CambiarEstadoAsync(int id, string estado, string actualizadoPor)
    {
        using var conn = db.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "sp_CambiarEstadoProducto",
            new { Id = id, Estado = estado, ActualizadoPor = actualizadoPor },
            commandType: CommandType.StoredProcedure);
        return rows > 0;
    }

    public async Task<IEnumerable<ProductoDto>> ListarBajoStockAsync(int umbral = 5)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryAsync<ProductoDto>(
            "sp_ListarProductosBajoStock",
            new { Umbral = umbral },
            commandType: CommandType.StoredProcedure);
    }
}
