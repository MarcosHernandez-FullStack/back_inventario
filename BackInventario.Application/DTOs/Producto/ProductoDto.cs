namespace BackInventario.Application.DTOs.Producto;

public class ProductoDto
{
    public int      Id             { get; set; }
    public string   Nombre         { get; set; } = string.Empty;
    public string?  Descripcion    { get; set; }
    public decimal  Precio         { get; set; }
    public int      Cantidad       { get; set; }
    public int      IdCategoria    { get; set; }
    public string   NombreCategoria { get; set; } = string.Empty;
    public string   Estado         { get; set; } = string.Empty;
    public DateTime? FechaCreacion  { get; set; }
}
