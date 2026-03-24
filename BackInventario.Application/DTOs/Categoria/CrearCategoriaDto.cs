using System.ComponentModel.DataAnnotations;

namespace BackInventario.Application.DTOs.Categoria;

public class CrearCategoriaDto
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string CreadoPor { get; set; } = string.Empty;
}
