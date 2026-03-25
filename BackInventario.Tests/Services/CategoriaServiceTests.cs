using BackInventario.Application.DTOs.Categoria;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;

namespace BackInventario.Tests.Services;

public class CategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _repoMock;
    private readonly CategoriaService _sut;

    public CategoriaServiceTests()
    {
        _repoMock = new Mock<ICategoriaRepository>();
        _sut = new CategoriaService(_repoMock.Object);
    }

    // ── ListarAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task ListarAsync_DebeRetornarListaDeCategorias()
    {
        // Arrange
        var categorias = new List<CategoriaDto>
        {
            new() { Id = 1, Nombre = "Electrónica", Estado = "ACTIVO" },
            new() { Id = 2, Nombre = "Ropa",        Estado = "ACTIVO" },
        };
        _repoMock.Setup(r => r.ListarAsync()).ReturnsAsync(categorias);

        // Act
        var resultado = await _sut.ListarAsync();

        // Assert
        Assert.Equal(2, resultado.Count());
        _repoMock.Verify(r => r.ListarAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarAsync_CuandoNoHayCategorias_RetornaListaVacia()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarAsync()).ReturnsAsync([]);

        // Act
        var resultado = await _sut.ListarAsync();

        // Assert
        Assert.Empty(resultado);
    }

    // ── ObtenerPorIdAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaCategoria()
    {
        // Arrange
        var categoria = new CategoriaDto { Id = 1, Nombre = "Electrónica", Estado = "ACTIVO" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(categoria);

        // Act
        var resultado = await _sut.ObtenerPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1,            resultado.Id);
        Assert.Equal("Electrónica", resultado.Nombre);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_RetornaNull()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((CategoriaDto?)null);

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
        var dto = new CrearCategoriaDto { Nombre = "Nueva", CreadoPor = 1 };
        _repoMock.Setup(r => r.CrearAsync(dto)).ReturnsAsync(5);

        // Act
        var id = await _sut.CrearAsync(dto);

        // Assert
        Assert.Equal(5, id);
        _repoMock.Verify(r => r.CrearAsync(dto), Times.Once);
    }

    // ── ActualizarAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task ActualizarAsync_CuandoExiste_RetornaTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarAsync(1, "NuevoNombre", 1)).ReturnsAsync(true);

        // Act
        var resultado = await _sut.ActualizarAsync(1, "NuevoNombre", 1);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_RetornaFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarAsync(99, It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var resultado = await _sut.ActualizarAsync(99, "X", 1);

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
}
