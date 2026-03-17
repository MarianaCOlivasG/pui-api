using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PUI.Api.DTOs.Reportes
{
    public class DesactivarReportePUIDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(75, MinimumLength = 36, ErrorMessage = "El campo '{0}' debe tener entre {2} y {1} caracteres.")]
        [Display(Name = "id")]
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!; 
    }
}