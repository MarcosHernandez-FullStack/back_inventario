namespace BackInventario.Application.DTOs.Usuario;

public class UsuarioDto
{
    public int      Id        { get; set; }
    public string   Nombres   { get; set; } = string.Empty;
    public string   Apellidos { get; set; } = string.Empty;
    public string?  Celular   { get; set; }
    public string   Correo    { get; set; } = string.Empty;
    public string   Rol       { get; set; } = string.Empty;
    public string   Estado    { get; set; } = string.Empty;
}
