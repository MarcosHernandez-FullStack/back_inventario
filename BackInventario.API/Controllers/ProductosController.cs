using BackInventario.Application.DTOs.Producto;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController(ProductoService productoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? nombre, [FromQuery] int? idCategoria)
        => Ok(await productoService.ListarAsync(nombre, idCategoria));

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await productoService.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpGet("bajo-stock")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> BajoStock([FromQuery] int umbral = 5)
        => Ok(await productoService.ListarBajoStockAsync(umbral));

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Crear([FromBody] CrearProductoDto dto)
    {
        var id = await productoService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoDto dto)
    {
        var ok = await productoService.ActualizarAsync(id, dto);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await productoService.EliminarAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoProductoDto dto)
    {
        var ok = await productoService.CambiarEstadoAsync(id, dto.Estado, dto.ActualizadoPor);
        return ok ? NoContent() : NotFound();
    }
}

public record CambiarEstadoProductoDto(string Estado, int ActualizadoPor);
