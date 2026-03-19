using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PUI.Api.Dtos.Eventos
{
    public class NuevaNotificacionPAMDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "El campo '{0}' debe tener {1} caracteres.")]
        [Display(Name = "curp")]
        [JsonPropertyName("curp")]
        public string Curp { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "nombre")]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "primer_apellido")]
        [JsonPropertyName("primer_apellido")]
        public string? PrimerApellido { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "segundo_apellido")]
        [JsonPropertyName("segundo_apellido")]
        public string? SegundoApellido { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "tipo_evento")]
        [JsonPropertyName("tipo_evento")]
        public string? TipoEvento { get; set; }

        [JsonPropertyName("fecha_evento")]
        public DateTime? FechaEvento { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "descripcion_lugar_evento")]
        [JsonPropertyName("descripcion_lugar_evento")]
        public string? DescripcionLugarEvento { get; set; }
    }
}
