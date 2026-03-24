using BackInventario.Application.DTOs.Categoria;
using BackInventario.Application.Interfaces;

namespace BackInventario.Application.Services;

public class CategoriaService(ICategoriaRepository categoriaRepository)
{
    public Task<IEnumerable<CategoriaDto>> ListarAsync()
        => categoriaRepository.ListarAsync();

    public Task<CategoriaDto?> ObtenerPorIdAsync(int id)
        => categoriaRepository.ObtenerPorIdAsync(id);

    public Task<int> CrearAsync(CrearCategoriaDto dto)
        => categoriaRepository.CrearAsync(dto);

    public Task<bool> ActualizarAsync(int id, string nombre, string actualizadoPor)
        => categoriaRepository.ActualizarAsync(id, nombre, actualizadoPor);

    public Task<bool> CambiarEstadoAsync(int id, string estado, string actualizadoPor)
        => categoriaRepository.CambiarEstadoAsync(id, estado, actualizadoPor);

    public Task<bool> EliminarAsync(int id)
        => categoriaRepository.EliminarAsync(id);
}
