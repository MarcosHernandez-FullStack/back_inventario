using BackInventario.API.Controllers;
using BackInventario.Application.DTOs.Usuario;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackInventario.Tests.Controllers;

public class UsuariosControllerTests
{
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly UsuariosController _sut;

    public UsuariosControllerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        var service = new UsuarioService(_repoMock.Object);
        _sut = new UsuariosController(service);
    }

    // ── Listar ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Listar_Siempre_Retorna200OkConLista()
    {
        // Arrange
        _repoMock.Setup(r => r.ListarAsync()).ReturnsAsync([
            new() { Id = 1, Nombres = "Juan",  Apellidos = "Pérez",  Correo = "juan@test.com",  Rol = "EMPLEADO",      Estado = "ACTIVO" },
            new() { Id = 2, Nombres = "María", Apellidos = "Gómez",  Correo = "maria@test.com", Rol = "ADMINISTRADOR", Estado = "ACTIVO" },
        ]);

        // Act
        var actionResult = await _sut.Listar();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ok.StatusCode);
    }

    // ── ObtenerPorId ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorId_CuandoExiste_Retorna200ConUsuario()
    {
        // Arrange
        var usuario = new UsuarioDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Correo = "juan@test.com", Rol = "EMPLEADO", Estado = "ACTIVO" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);

        // Act
        var actionResult = await _sut.ObtenerPorId(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        var dto = Assert.IsType<UsuarioDto>(ok.Value);
        Assert.Equal("Juan", dto.Nombres);
        Assert.Equal("EMPLEADO", dto.Rol);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((UsuarioDto?)null);

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
        var dto = new CrearUsuarioDto
        {
            Nombres    = "Carlos",
            Apellidos  = "Ruiz",
            Correo     = "carlos@test.com",
            Contrasena = "secreta123",
            Rol        = "EMPLEADO",
            CreadoPor  = 1,
        };
        _repoMock.Setup(r => r.CrearAsync(dto)).ReturnsAsync(3);

        // Act
        var actionResult = await _sut.Crear(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(actionResult);
        Assert.Equal(201, created.StatusCode);
    }

    // ── ActualizarEstado ──────────────────────────────────────────────────────

    [Fact]
    public async Task ActualizarEstado_CuandoExiste_Retorna204NoContent()
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarEstadoAsync(1, "INACTIVO", 1)).ReturnsAsync(true);

        // Act
        var actionResult = await _sut.ActualizarEstado(1, new ActualizarEstadoDto("INACTIVO", 1));

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task ActualizarEstado_CuandoNoExiste_Retorna404NotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarEstadoAsync(99, It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var actionResult = await _sut.ActualizarEstado(99, new ActualizarEstadoDto("ACTIVO", 1));

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }
}
