using BackInventario.Application.DTOs.Usuario;

namespace BackInventario.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<UsuarioDto>> ListarAsync();
    Task<UsuarioDto?>             ObtenerPorIdAsync(int id);
    Task<int>                     CrearAsync(CrearUsuarioDto dto);
    Task<bool>                    ActualizarEstadoAsync(int id, string estado, string actualizadoPor);
}
