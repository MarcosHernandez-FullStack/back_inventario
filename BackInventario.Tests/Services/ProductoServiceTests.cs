using BackInventario.Application.DTOs.Producto;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;

namespace BackInventario.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _repoMock;
    private readonly ProductoService _sut;

    public ProductoServiceTests()
    {
        _repoMock = new Mock<IProductoRepository>();
        _sut = new ProductoService(_repoMock.Object);
    }

    // ── ListarAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task ListarAsync_SinFiltros_RetornaListaCompleta()
    {
        // Arrange
        var productos = new List<ProductoDto>
        {
            new() { Id = 1, Nombre = "Laptop",  Precio = 2500m, Cantidad = 10, Estado = "ACTIVO" },
            new() { Id = 2, Nombre = "Monitor", Precio = 800m,  Cantidad = 5,  Estado = "ACTIVO" },
        };
        _repoMock.Setup(r => r.ListarAsync(null, null)).ReturnsAsync(productos);

        // Act
        var resultado = await _sut.ListarAsync(null, null);

        // Assert
        Assert.Equal(2, resultado.Count());
        _repoMock.Verify(r => r.ListarAsync(null, null), Times.Once);
    }

    [Fact]
    public async Task ListarAsync_ConFiltroNombre_LlamaoRepoConNombre()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarAsync("Laptop", null)).ReturnsAsync([
            new() { Id = 1, Nombre = "Laptop", Precio = 2500m, Cantidad = 10, Estado = "ACTIVO" }
        ]);

        // Act
        var resultado = await _sut.ListarAsync("Laptop", null);

        // Assert
        Assert.Single(resultado);
        _repoMock.Verify(r => r.ListarAsync("Laptop", null), Times.Once);
    }

    // ── ObtenerPorIdAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaProducto()
    {
        // Arrange
        var producto = new ProductoDto { Id = 1, Nombre = "Laptop", Precio = 2500m, Cantidad = 10, Estado = "ACTIVO" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        // Act
        var resultado = await _sut.ObtenerPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Laptop", resultado.Nombre);
        Assert.Equal(2500m,    resultado.Precio);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_RetornaNull()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((ProductoDto?)null);

        // Act
        var resultado = await _sut.ObtenerPorIdAsync(99);

        // Assert
        Assert.Null(resultado);
    }

    // ── CrearAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task CrearAsync_DebeRetornarIdGenerado()
    {
        // Arrange
        var dto = new CrearProductoDto
        {
            Nombre      = "Nuevo Producto",
            Precio      = 100m,
            Cantidad    = 10,
            IdCategoria = 1,
            CreadoPor   = 1,
        };
        _repoMock.Setup(r => r.CrearAsync(dto)).ReturnsAsync(7);

        // Act
        var id = await _sut.CrearAsync(dto);

        // Assert
        Assert.Equal(7, id);
        _repoMock.Verify(r => r.CrearAsync(dto), Times.Once);
    }

    // ── ActualizarAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task ActualizarAsync_CuandoExiste_RetornaTrue()
    {
        // Arrange
        var dto = new ActualizarProductoDto { Nombre = "Laptop Pro", Precio = 3000m, Cantidad = 8, IdCategoria = 1, ActualizadoPor = 1 };
        _repoMock.Setup(r => r.ActualizarAsync(1, dto)).ReturnsAsync(true);

        // Act
        var resultado = await _sut.ActualizarAsync(1, dto);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_RetornaFalse()
    {
        // Arrange
        var dto = new ActualizarProductoDto { Nombre = "X", Precio = 1m, Cantidad = 1, IdCategoria = 1, ActualizadoPor = 1 };
        _repoMock.Setup(r => r.ActualizarAsync(99, dto)).ReturnsAsync(false);

        // Act
        var resultado = await _sut.ActualizarAsync(99, dto);

        // Assert
        Assert.False(resultado);
    }

    // ── EliminarAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task EliminarAsync_CuandoExiste_RetornaTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.EliminarAsync(1)).ReturnsAsync(true);

        // Act
        var resultado = await _sut.EliminarAsync(1);

        // Assert
        Assert.True(resultado);
        _repoMock.Verify(r => r.EliminarAsync(1), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_CuandoNoExiste_RetornaFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.EliminarAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _sut.EliminarAsync(99);

        // Assert
        Assert.False(resultado);
    }

    // ── CambiarEstadoAsync ───────────────────────────────────────────────────

    [Theory]
    [InlineData("ACTIVO")]
    [InlineData("INACTIVO")]
    public async Task CambiarEstadoAsync_CuandoExiste_RetornaTrue(string nuevoEstado)
    {
        // Arrange
        _repoMock.Setup(r => r.CambiarEstadoAsync(1, nuevoEstado, 1)).ReturnsAsync(true);

        // Act
        var resultado = await _sut.CambiarEstadoAsync(1, nuevoEstado, 1);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task CambiarEstadoAsync_CuandoNoExiste_RetornaFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.CambiarEstadoAsync(99, It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var resultado = await _sut.CambiarEstadoAsync(99, "ACTIVO", 1);

        // Assert
        Assert.False(resultado);
    }

    // ── ListarBajoStockAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task ListarBajoStockAsync_RetornaProductosConStockBajo()
    {
        // Arrange
        var productosAlerta = new List<ProductoDto>
        {
            new() { Id = 3, Nombre = "Teclado", Cantidad = 2, Estado = "ACTIVO" },
            new() { Id = 6, Nombre = "Auriculares", Cantidad = 1, Estado = "ACTIVO" },
        };
        _repoMock.Setup(r => r.ListarBajoStockAsync(5)).ReturnsAsync(productosAlerta);

        // Act
        var resultado = await _sut.ListarBajoStockAsync(5);

        // Assert
        Assert.Equal(2, resultado.Count());
        Assert.All(resultado, p => Assert.True(p.Cantidad <= 5));
    }

    [Fact]
    public async Task ListarBajoStockAsync_UmbralPorDefecto_UsaValor5()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarBajoStockAsync(5)).ReturnsAsync([]);

        // Act
        await _sut.ListarBajoStockAsync();

        // Assert
        _repoMock.Verify(r => r.ListarBajoStockAsync(5), Times.Once);
    }
}
