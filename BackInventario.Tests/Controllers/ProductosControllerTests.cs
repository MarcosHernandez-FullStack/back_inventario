using BackInventario.API.Controllers;
using BackInventario.Application.DTOs.Producto;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.Tests.Controllers;

public class ProductosControllerTests
{
    private readonly Mock<IProductoRepository> _repoMock;
    private readonly ProductosController _sut;

    public ProductosControllerTests()
    {
        _repoMock = new Mock<IProductoRepository>();
        var service = new ProductoService(_repoMock.Object);
        _sut = new ProductosController(service);
    }

    // ── Listar ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Listar_SinFiltros_Retorna200OkConLista()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarAsync(null, null)).ReturnsAsync([
            new() { Id = 1, Nombre = "Laptop",  Precio = 2500m, Cantidad = 10, Estado = "ACTIVO" },
            new() { Id = 2, Nombre = "Monitor", Precio = 800m,  Cantidad = 5,  Estado = "INACTIVO" },
        ]);

        // Act
        var actionResult = await _sut.Listar(null, null);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ok.StatusCode);
    }

    // ── ObtenerPorId ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorId_CuandoExiste_Retorna200ConProducto()
    {
        // Arrange
        var producto = new ProductoDto { Id = 1, Nombre = "Laptop", Precio = 2500m, Cantidad = 10, Estado = "ACTIVO" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        // Act
        var actionResult = await _sut.ObtenerPorId(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        var dto = Assert.IsType<ProductoDto>(ok.Value);
        Assert.Equal("Laptop", dto.Nombre);
        Assert.Equal(2500m,    dto.Precio);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((ProductoDto?)null);

        // Act
        var actionResult = await _sut.ObtenerPorId(99);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    // ── BajoStock ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task BajoStock_ConUmbralPorDefecto_Retorna200ConProductosAlerta()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarBajoStockAsync(5)).ReturnsAsync([
            new() { Id = 3, Nombre = "Teclado",     Cantidad = 2, Estado = "ACTIVO" },
            new() { Id = 6, Nombre = "Auriculares", Cantidad = 1, Estado = "ACTIVO" },
        ]);

        // Act
        var actionResult = await _sut.BajoStock(5);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ok.StatusCode);
    }

    // ── Crear ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Crear_ConDatosValidos_Retorna201Created()
    {
        // Arrange
        var dto = new CrearProductoDto { Nombre = "Nuevo", Precio = 100m, Cantidad = 5, IdCategoria = 1, CreadoPor = "admin@test.com" };
        _repoMock.Setup(r => r.CrearAsync(dto)).ReturnsAsync(10);

        // Act
        var actionResult = await _sut.Crear(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(actionResult);
        Assert.Equal(201, created.StatusCode);
    }

    // ── Actualizar ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Actualizar_CuandoExiste_Retorna204NoContent()
    {
        // Arrange
        var dto = new ActualizarProductoDto { Nombre = "Laptop Pro", Precio = 3000m, Cantidad = 8, IdCategoria = 1, ActualizadoPor = "admin@test.com" };
        _repoMock.Setup(r => r.ActualizarAsync(1, dto)).ReturnsAsync(true);

        // Act
        var actionResult = await _sut.Actualizar(1, dto);

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task Actualizar_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        var dto = new ActualizarProductoDto { Nombre = "X", Precio = 1m, Cantidad = 1, IdCategoria = 1, ActualizadoPor = "admin@test.com" };
        _repoMock.Setup(r => r.ActualizarAsync(99, dto)).ReturnsAsync(false);

        // Act
        var actionResult = await _sut.Actualizar(99, dto);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    // ── Eliminar ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task Eliminar_CuandoExiste_Retorna204NoContent()
    {
        // Arrange
        _repoMock.Setup(r => r.EliminarAsync(1)).ReturnsAsync(true);

        // Act
        var actionResult = await _sut.Eliminar(1);

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.EliminarAsync(99)).ReturnsAsync(false);

        // Act
        var actionResult = await _sut.Eliminar(99);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    // ── CambiarEstado ─────────────────────────────────────────────────────────

    [Fact]
    public async Task CambiarEstado_CuandoExiste_Retorna204NoContent()
    {
        // Arrange
        _repoMock.Setup(r => r.CambiarEstadoAsync(1, "INACTIVO", "admin@test.com")).ReturnsAsync(true);

        // Act
        var actionResult = await _sut.CambiarEstado(1, new CambiarEstadoProductoDto("INACTIVO", "admin@test.com"));

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task CambiarEstado_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.CambiarEstadoAsync(99, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var actionResult = await _sut.CambiarEstado(99, new CambiarEstadoProductoDto("ACTIVO", "admin@test.com"));

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }
}
