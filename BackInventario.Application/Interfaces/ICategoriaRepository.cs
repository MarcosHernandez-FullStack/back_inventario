using BackInventario.Application.DTOs.Categoria;

namespace BackInventario.Application.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<CategoriaDto>> ListarAsync();
    Task<CategoriaDto?>             ObtenerPorIdAsync(int id);
    Task<int>                       CrearAsync(CrearCategoriaDto dto);
    Task<bool>                      ActualizarAsync(int id, string nombre, string actualizadoPor);
    Task<bool>                      CambiarEstadoAsync(int id, string estado, string actualizadoPor);
    Task<bool>                      EliminarAsync(int id);
}
