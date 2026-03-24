using BackInventario.Application.DTOs.Reporte;
using BackInventario.Application.Interfaces;

namespace BackInventario.Application.Services;

public class ReporteService(IReporteRepository reporteRepository)
{
    public Task<ResumenDto>                        ResumenAsync()           => reporteRepository.ResumenAsync();
    public Task<IEnumerable<StockPorCategoriaDto>> StockPorCategoriaAsync() => reporteRepository.StockPorCategoriaAsync();
}
