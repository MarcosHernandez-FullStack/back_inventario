using BackInventario.API.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackInventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacionesController(IHubContext<NotificacionHub> hub) : ControllerBase
{
    [HttpPost("reportar")]
    [Authorize(Roles = "EMPLEADO")]
    public async Task<IActionResult> Reportar([FromBody] ReporteStockDto dto)
    {
        await hub.Clients.Group("ADMINISTRADOR").SendAsync("StockBajo", new
        {
            mensaje      = $"El empleado {dto.ReportadoPor} reportó inventario bajo.",
            productos    = dto.Productos,
            reportadoPor = dto.ReportadoPor,
            fecha        = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
        });

        return Ok();
    }
}

public record ReporteStockDto(string ReportadoPor, List<ProductoAlertaDto> Productos);
public record ProductoAlertaDto(string Nombre, int Cantidad);
