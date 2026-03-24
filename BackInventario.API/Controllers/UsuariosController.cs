using BackInventario.Application.DTOs.Usuario;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMINISTRADOR")]
public class UsuariosController(UsuarioService usuarioService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar()
        => Ok(await usuarioService.ListarAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario = await usuarioService.ObtenerPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioDto dto)
    {
        var id = await usuarioService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id });
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoDto dto)
    {
        var ok = await usuarioService.ActualizarEstadoAsync(id, dto.Estado, dto.ActualizadoPor);
        return ok ? NoContent() : NotFound();
    }
}

public record ActualizarEstadoDto(string Estado, string ActualizadoPor);
