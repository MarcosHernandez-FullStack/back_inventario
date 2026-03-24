using BackInventario.Application.DTOs.Usuario;
using BackInventario.Application.Interfaces;

namespace BackInventario.Application.Services;

public class UsuarioService(IUsuarioRepository usuarioRepository)
{
    public Task<IEnumerable<UsuarioDto>> ListarAsync()
        => usuarioRepository.ListarAsync();

    public Task<UsuarioDto?> ObtenerPorIdAsync(int id)
        => usuarioRepository.ObtenerPorIdAsync(id);

    public Task<int> CrearAsync(CrearUsuarioDto dto)
        => usuarioRepository.CrearAsync(dto);

    public Task<bool> ActualizarEstadoAsync(int id, string estado, int actualizadoPor)
        => usuarioRepository.ActualizarEstadoAsync(id, estado, actualizadoPor);
}
