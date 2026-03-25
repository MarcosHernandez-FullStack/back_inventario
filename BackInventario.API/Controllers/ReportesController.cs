using BackInventario.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController(ReporteService reporteService, ProductoService productoService) : ControllerBase
{
    [HttpGet("resumen")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Resumen()
        => Ok(await reporteService.ResumenAsync());

    [HttpGet("stock-por-categoria")]
    public async Task<IActionResult> StockPorCategoria()
        => Ok(await reporteService.StockPorCategoriaAsync());

    [HttpGet("bajo-stock")]
    public async Task<IActionResult> BajoStock([FromQuery] int umbral = 5)
        => Ok(await productoService.ListarBajoStockAsync(umbral));
}
