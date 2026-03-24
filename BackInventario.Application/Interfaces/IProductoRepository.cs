using BackInventario.Application.DTOs.Producto;

namespace BackInventario.Application.Interfaces;

public interface IProductoRepository
{
    Task<IEnumerable<ProductoDto>> ListarAsync(string? nombre, int? idCategoria);
    Task<ProductoDto?>             ObtenerPorIdAsync(int id);
    Task<int>                      CrearAsync(CrearProductoDto dto);
    Task<bool>                     ActualizarAsync(int id, ActualizarProductoDto dto);
    Task<bool>                     EliminarAsync(int id);
    Task<bool>                     CambiarEstadoAsync(int id, string estado, string actualizadoPor);
    Task<IEnumerable<ProductoDto>> ListarBajoStockAsync(int umbral = 5);
}
