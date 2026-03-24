using BackInventario.Application.DTOs.Auth;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;

namespace BackInventario.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _repoMock;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _repoMock = new Mock<IAuthRepository>();
        _sut = new AuthService(_repoMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ConCredencialesValidas_RetornaLoginResponse()
    {
        // Arrange
        var request = new LoginRequestDto { Correo = "admin@test.com", Contrasena = "pass123" };
        var response = new LoginResponseDto
        {
            Id        = 1,
            Nombres   = "Admin",
            Apellidos = "Sistema",
            Correo    = "admin@test.com",
            Rol       = "ADMINISTRADOR",
        };
        _repoMock.Setup(r => r.LoginAsync(request)).ReturnsAsync(response);

        // Act
        var resultado = await _sut.LoginAsync(request);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("admin@test.com", resultado.Correo);
        Assert.Equal("ADMINISTRADOR",  resultado.Rol);
        _repoMock.Verify(r => r.LoginAsync(request), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ConCredencialesInvalidas_RetornaNull()
    {
        // Arrange
        var request = new LoginRequestDto { Correo = "noexiste@test.com", Contrasena = "wrongpass" };
        _repoMock.Setup(r => r.LoginAsync(request)).ReturnsAsync((LoginResponseDto?)null);

        // Act
        var resultado = await _sut.LoginAsync(request);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task LoginAsync_LlamaAlRepositorioUnaVez()
    {
        // Arrange
        var request = new LoginRequestDto { Correo = "x@test.com", Contrasena = "x" };
        _repoMock.Setup(r => r.LoginAsync(It.IsAny<LoginRequestDto>())).ReturnsAsync((LoginResponseDto?)null);

        // Act
        await _sut.LoginAsync(request);

        // Assert
        _repoMock.Verify(r => r.LoginAsync(It.IsAny<LoginRequestDto>()), Times.Once);
    }
}
