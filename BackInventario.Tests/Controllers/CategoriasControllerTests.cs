using BackInventario.API.Controllers;
using BackInventario.Application.DTOs.Categoria;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.Tests.Controllers;

public class CategoriasControllerTests
{
    private readonly Mock<ICategoriaRepository> _repoMock;
    private readonly CategoriasController _sut;

    public CategoriasControllerTests()
    {
        _repoMock = new Mock<ICategoriaRepository>();
        var service = new CategoriaService(_repoMock.Object);
        _sut = new CategoriasController(service);
    }

    // ── Listar ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Listar_Siempre_Retorna200OkConLista()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarAsync()).ReturnsAsync([
            new() { Id = 1, Nombre = "Electrónica", Estado = "ACTIVO" },
        ]);

        // Act
        var actionResult = await _sut.Listar();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ok.StatusCode);
    }

    // ── ObtenerPorId ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorId_CuandoExiste_Retorna200Ok()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1))
                 .ReturnsAsync(new CategoriaDto { Id = 1, Nombre = "Electrónica", Estado = "ACTIVO" });

        // Act
        var actionResult = await _sut.ObtenerPorId(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ok.StatusCode);
        var dto = Assert.IsType<CategoriaDto>(ok.Value);
        Assert.Equal("Electrónica", dto.Nombre);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((CategoriaDto?)null);

        // Act
        var actionResult = await _sut.ObtenerPorId(99);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    // ── Crear ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Crear_ConDatosValidos_Retorna201Created()
    {
        // Arrange
        var dto = new CrearCategoriaDto { Nombre = "Nueva", CreadoPor = 1 };
        _repoMock.Setup(r => r.CrearAsync(dto)).ReturnsAsync(5);

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
        _repoMock.Setup(r => r.ActualizarAsync(1, "NuevoNombre", 1)).ReturnsAsync(true);

        // Act
        var actionResult = await _sut.Actualizar(1, new ActualizarCategoriaDto("NuevoNombre", 1));

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task Actualizar_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarAsync(99, It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var actionResult = await _sut.Actualizar(99, new ActualizarCategoriaDto("X", 1));

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
}
