namespace BackInventario.Domain.Entities;

public class Producto
{
    public int       Id                  { get; set; }
    public string    Nombre              { get; set; } = string.Empty;
    public string?   Descripcion         { get; set; }
    public decimal   Precio              { get; set; }
    public int       Cantidad            { get; set; }
    public int       IdCategoria         { get; set; }
    public string    Estado              { get; set; } = string.Empty;
    public DateTime  FechaCreacion       { get; set; }
    public DateTime? FechaActualizacion  { get; set; }
    public string    CreadoPor           { get; set; } = string.Empty;
    public string?   ActualizadoPor      { get; set; }
}
