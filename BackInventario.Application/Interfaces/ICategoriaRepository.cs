using BackInventario.Application.DTOs.Categoria;

namespace BackInventario.Application.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<CategoriaDto>> ListarAsync();
    Task<CategoriaDto?>             ObtenerPorIdAsync(int id);
    Task<int>                       CrearAsync(CrearCategoriaDto dto);
    Task<bool>                      ActualizarAsync(int id, string nombre, int actualizadoPor);
    Task<bool>                      CambiarEstadoAsync(int id, string estado, int actualizadoPor);
    Task<bool>                      EliminarAsync(int id);
}
