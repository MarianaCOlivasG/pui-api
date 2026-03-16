using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PUI.Api.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo '{0}' debe tener entre {2} y {1} caracteres.")]
        [Display(Name = "usuario")]
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(20, MinimumLength = 16, ErrorMessage = "El campo '{0}' debe tener entre {2} y {1} caracteres.")]
        [Display(Name = "clave")]
        [JsonPropertyName("clave")]
        public string Clave { get; set; } = null!;

     
    }
}