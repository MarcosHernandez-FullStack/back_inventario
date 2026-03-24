using BackInventario.Application.DTOs.Reporte;

namespace BackInventario.Application.Interfaces;

public interface IReporteRepository
{
    Task<ResumenDto>                      ResumenAsync();
    Task<IEnumerable<StockPorCategoriaDto>> StockPorCategoriaAsync();
}
