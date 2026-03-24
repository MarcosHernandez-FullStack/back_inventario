using BackInventario.Application.DTOs.Producto;
using BackInventario.Application.Interfaces;

namespace BackInventario.Application.Services;

public class ProductoService(IProductoRepository productoRepository)
{
    public Task<IEnumerable<ProductoDto>> ListarAsync(string? nombre, int? idCategoria)
        => productoRepository.ListarAsync(nombre, idCategoria);

    public Task<ProductoDto?> ObtenerPorIdAsync(int id)
        => productoRepository.ObtenerPorIdAsync(id);

    public Task<int> CrearAsync(CrearProductoDto dto)
        => productoRepository.CrearAsync(dto);

    public Task<bool> ActualizarAsync(int id, ActualizarProductoDto dto)
        => productoRepository.ActualizarAsync(id, dto);

    public Task<bool> EliminarAsync(int id)
        => productoRepository.EliminarAsync(id);

    public Task<bool> CambiarEstadoAsync(int id, string estado, int actualizadoPor)
        => productoRepository.CambiarEstadoAsync(id, estado, actualizadoPor);

    public Task<IEnumerable<ProductoDto>> ListarBajoStockAsync(int umbral = 5)
        => productoRepository.ListarBajoStockAsync(umbral);
}
