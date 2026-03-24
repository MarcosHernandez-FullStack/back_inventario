namespace BackInventario.Application.DTOs.Auth;

public class LoginResponseDto
{
    public string Token     { get; set; } = string.Empty;
    public int    Id        { get; set; }
    public string Nombres   { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Correo    { get; set; } = string.Empty;
    public string Rol       { get; set; } = string.Empty;
}
