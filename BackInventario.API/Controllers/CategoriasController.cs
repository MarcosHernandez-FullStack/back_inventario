using BackInventario.Application.DTOs.Categoria;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriasController(CategoriaService categoriaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar()
        => Ok(await categoriaService.ListarAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var categoria = await categoriaService.ObtenerPorIdAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
    {
        var id = await categoriaService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCategoriaDto dto)
    {
        var ok = await categoriaService.ActualizarAsync(id, dto.Nombre, dto.ActualizadoPor);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoCategoriaDto dto)
    {
        var ok = await categoriaService.CambiarEstadoAsync(id, dto.Estado, dto.ActualizadoPor);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await categoriaService.EliminarAsync(id);
        return ok ? NoContent() : NotFound();
    }
}

public record ActualizarCategoriaDto(string Nombre, int ActualizadoPor);
public record CambiarEstadoCategoriaDto(string Estado, int ActualizadoPor);
