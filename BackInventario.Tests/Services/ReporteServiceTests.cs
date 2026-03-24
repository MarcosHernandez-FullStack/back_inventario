using BackInventario.Application.DTOs.Reporte;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;

namespace BackInventario.Tests.Services;

public class ReporteServiceTests
{
    private readonly Mock<IReporteRepository> _repoMock;
    private readonly ReporteService _sut;

    public ReporteServiceTests()
    {
        _repoMock = new Mock<IReporteRepository>();
        _sut = new ReporteService(_repoMock.Object);
    }

    // ── ResumenAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ResumenAsync_RetornaResumenConDatosDelRepositorio()
    {
        // Arrange
        var resumen = new ResumenDto
        {
            TotalProductos  = 20,
            TotalCategorias = 5,
            TotalUsuarios   = 8,
            ValorInventario = 45000m,
        };
        _repoMock.Setup(r => r.ResumenAsync()).ReturnsAsync(resumen);

        // Act
        var resultado = await _sut.ResumenAsync();

        // Assert
        Assert.Equal(20,      resultado.TotalProductos);
        Assert.Equal(5,       resultado.TotalCategorias);
        Assert.Equal(8,       resultado.TotalUsuarios);
        Assert.Equal(45000m,  resultado.ValorInventario);
        _repoMock.Verify(r => r.ResumenAsync(), Times.Once);
    }

    [Fact]
    public async Task ResumenAsync_CuandoInventarioVacio_RetornaValoresCero()
    {
        // Arrange
        var resumen = new ResumenDto { TotalProductos = 0, TotalCategorias = 0, TotalUsuarios = 0, ValorInventario = 0m };
        _repoMock.Setup(r => r.ResumenAsync()).ReturnsAsync(resumen);

        // Act
        var resultado = await _sut.ResumenAsync();

        // Assert
        Assert.Equal(0,  resultado.TotalProductos);
        Assert.Equal(0m, resultado.ValorInventario);
    }

    // ── StockPorCategoriaAsync ───────────────────────────────────────────────

    [Fact]
    public async Task StockPorCategoriaAsync_RetornaListaDeCategorias()
    {
        // Arrange
        var stock = new List<StockPorCategoriaDto>
        {
            new() { Categoria = "Electrónica", TotalProductos = 5, StockTotal = 30, ValorTotal = 15000m },
            new() { Categoria = "Periféricos", TotalProductos = 3, StockTotal = 25, ValorTotal = 2000m  },
        };
        _repoMock.Setup(r => r.StockPorCategoriaAsync()).ReturnsAsync(stock);

        // Act
        var resultado = await _sut.StockPorCategoriaAsync();

        // Assert
        Assert.Equal(2, resultado.Count());
        _repoMock.Verify(r => r.StockPorCategoriaAsync(), Times.Once);
    }

    [Fact]
    public async Task StockPorCategoriaAsync_CuandoNoHayDatos_RetornaListaVacia()
    {
        // Arrange
        _repoMock.Setup(r => r.StockPorCategoriaAsync()).ReturnsAsync([]);

        // Act
        var resultado = await _sut.StockPorCategoriaAsync();

        // Assert
        Assert.Empty(resultado);
    }
}
