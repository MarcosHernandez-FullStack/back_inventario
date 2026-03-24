namespace BackInventario.Domain.Entities;

public class Usuario
{
    public int       Id                  { get; set; }
    public string    Nombres             { get; set; } = string.Empty;
    public string    Apellidos           { get; set; } = string.Empty;
    public string?   Celular             { get; set; }
    public string    Correo              { get; set; } = string.Empty;
    public string    Contrasena          { get; set; } = string.Empty;
    public string    Rol                 { get; set; } = string.Empty;
    public string    Estado              { get; set; } = string.Empty;
    public DateTime  FechaCreacion       { get; set; }
    public DateTime? FechaActualizacion  { get; set; }
    public string    CreadoPor           { get; set; } = string.Empty;
    public string?   ActualizadoPor      { get; set; }
}
