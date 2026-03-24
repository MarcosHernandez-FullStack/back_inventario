using System.Data;
using BackInventario.Application.DTOs.Reporte;
using BackInventario.Application.Interfaces;
using BackInventario.Infrastructure.Data;
using Dapper;

namespace BackInventario.Infrastructure.Repositories;

public class ReporteRepository(DbConnectionFactory db) : IReporteRepository
{
    public async Task<ResumenDto> ResumenAsync()
    {
        using var conn = db.CreateConnection();
        return await conn.QueryFirstAsync<ResumenDto>(
            "sp_ResumenDashboard", commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<StockPorCategoriaDto>> StockPorCategoriaAsync()
    {
        using var conn = db.CreateConnection();
        return await conn.QueryAsync<StockPorCategoriaDto>(
            "sp_ReporteStockPorCategoria", commandType: CommandType.StoredProcedure);
    }
}
