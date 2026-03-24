namespace BackInventario.Application.DTOs.Reporte;

public class ResumenDto
{
    public int     TotalProductos  { get; set; }
    public int     TotalCategorias { get; set; }
    public int     TotalUsuarios   { get; set; }
    public decimal ValorInventario { get; set; }
}
