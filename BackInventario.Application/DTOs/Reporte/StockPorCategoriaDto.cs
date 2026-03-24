namespace BackInventario.Application.DTOs.Reporte;

public class StockPorCategoriaDto
{
    public string  Categoria      { get; set; } = string.Empty;
    public int     TotalProductos { get; set; }
    public int     StockTotal     { get; set; }
    public decimal ValorTotal     { get; set; }
}
