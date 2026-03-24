using BackInventario.Application.DTOs.Usuario;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;

namespace BackInventario.Tests.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly UsuarioService _sut;

    public UsuarioServiceTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        _sut = new UsuarioService(_repoMock.Object);
    }

    // ── ListarAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task ListarAsync_RetornaListaDeUsuarios()
    {
        // Arrange
        var usuarios = new List<UsuarioDto>
        {
            new() { Id = 1, Nombres = "Juan",  Apellidos = "Pérez",  Correo = "juan@test.com",  Rol = "EMPLEADO",       Estado = "ACTIVO" },
            new() { Id = 2, Nombres = "María", Apellidos = "Gómez",  Correo = "maria@test.com", Rol = "ADMINISTRADOR",  Estado = "ACTIVO" },
        };
        _repoMock.Setup(r => r.ListarAsync()).ReturnsAsync(usuarios);

        // Act
        var resultado = await _sut.ListarAsync();

        // Assert
        Assert.Equal(2, resultado.Count());
        _repoMock.Verify(r => r.ListarAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarAsync_CuandoNoHayUsuarios_RetornaListaVacia()
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
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaUsuario()
    {
        // Arrange
        var usuario = new UsuarioDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Correo = "juan@test.com", Rol = "EMPLEADO", Estado = "ACTIVO" };
        _repoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);

        // Act
        var resultado = await _sut.ObtenerPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1,     resultado.Id);
        Assert.Equal("Juan", resultado.Nombres);
        Assert.Equal("EMPLEADO", resultado.Rol);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_RetornaNull()
    {
        // Arrange
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((UsuarioDto?)null);

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
        var dto = new CrearUsuarioDto
        {
            Nombres    = "Carlos",
            Apellidos  = "Ruiz",
            Correo     = "carlos@test.com",
            Contrasena = "secreta123",
            Rol        = "EMPLEADO",
            CreadoPor  = "admin@test.com",
        };
        _repoMock.Setup(r => r.CrearAsync(dto)).ReturnsAsync(3);

        // Act
        var id = await _sut.CrearAsync(dto);

        // Assert
        Assert.Equal(3, id);
        _repoMock.Verify(r => r.CrearAsync(dto), Times.Once);
    }

    // ── ActualizarEstadoAsync ────────────────────────────────────────────────

    [Theory]
    [InlineData("ACTIVO")]
    [InlineData("INACTIVO")]
    public async Task ActualizarEstadoAsync_CuandoExiste_RetornaTrue(string estado)
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarEstadoAsync(1, estado, "admin@test.com")).ReturnsAsync(true);

        // Act
        var resultado = await _sut.ActualizarEstadoAsync(1, estado, "admin@test.com");

        // Assert
        Assert.True(resultado);
        _repoMock.Verify(r => r.ActualizarEstadoAsync(1, estado, "admin@test.com"), Times.Once);
    }

    [Fact]
    public async Task ActualizarEstadoAsync_CuandoNoExiste_RetornaFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.ActualizarEstadoAsync(99, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var resultado = await _sut.ActualizarEstadoAsync(99, "ACTIVO", "admin@test.com");

        // Assert
        Assert.False(resultado);
    }
}
