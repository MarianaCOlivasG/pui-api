using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PUI.Api.DTOs.Auth
{
    public class LoginPuiDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo '{0}' debe tener exactamente {1} caracteres.")]
        [RegularExpression("^(?i)PUI$", ErrorMessage = "El valor de '{0}' debe ser 'PUI'.")]
        [Display(Name = "usuario")]
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(20, MinimumLength = 16, ErrorMessage = "El campo '{0}' debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*\(\)\-_\.\+]).{16,20}$",
            ErrorMessage = "El campo '{0}' debe tener al menos una letra mayúscula, un número y uno de los caracteres especiales !@#$%^&*()-_ .+")]
        [Display(Name = "clave")]
        [JsonPropertyName("clave")]
        public string Pass { get; set; } = null!;
    }
}