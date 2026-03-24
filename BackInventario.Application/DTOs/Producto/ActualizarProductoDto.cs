using System.ComponentModel.DataAnnotations;

namespace BackInventario.Application.DTOs.Producto;

public class ActualizarProductoDto
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Range(typeof(decimal), "0.01", "99999.99", ErrorMessage = "El precio debe estar entre 0.01 y 99,999.99.")]
    public decimal Precio { get; set; }

    [Range(0, 99999, ErrorMessage = "La cantidad debe estar entre 0 y 99,999.")]
    public int Cantidad { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoría válida.")]
    public int IdCategoria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El usuario que actualiza es requerido.")]
    public int ActualizadoPor { get; set; }
}
