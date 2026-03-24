using System.ComponentModel.DataAnnotations;

namespace BackInventario.Application.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "El correo es requerido.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar 150 caracteres.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 50 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;
}
