namespace BackInventario.Domain.Entities;

public class Categoria
{
    public int       Id                  { get; set; }
    public string    Nombre              { get; set; } = string.Empty;
    public string    Estado              { get; set; } = string.Empty;
    public DateTime  FechaCreacion       { get; set; }
    public DateTime? FechaActualizacion  { get; set; }
    public string    CreadoPor           { get; set; } = string.Empty;
    public string?   ActualizadoPor      { get; set; }
}
