

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PUI.Api.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Display(Name = "usuario")]
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Display(Name = "pass")]
        [JsonPropertyName("pass")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula y un número."
        )]
        public string Pass { get; set; } = null!;
    }
}