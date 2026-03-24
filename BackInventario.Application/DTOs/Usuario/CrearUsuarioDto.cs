using System.ComponentModel.DataAnnotations;

namespace BackInventario.Application.DTOs.Usuario;

public class CrearUsuarioDto
{
    [Required(ErrorMessage = "Los nombres son requeridos.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son requeridos.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El número de celular no es válido.")]
    [StringLength(20)]
    public string? Celular { get; set; }

    [Required(ErrorMessage = "El correo es requerido.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar 150 caracteres.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 50 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido.")]
    [RegularExpression("^(EMPLEADO|ADMINISTRADOR)$", ErrorMessage = "El rol debe ser EMPLEADO o ADMINISTRADOR.")]
    public string Rol { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El usuario que crea es requerido.")]
    public int CreadoPor { get; set; }
}
