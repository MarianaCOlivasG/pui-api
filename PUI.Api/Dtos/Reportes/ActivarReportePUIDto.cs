using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PUI.Api.DTOs.Reportes
{
    public class ActivarReportePUIDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(75, MinimumLength = 36, ErrorMessage = "El campo '{0}' debe tener entre {2} y {1} caracteres.")]
        [Display(Name = "id")]
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;


        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "El campo '{0}' debe tener {1} caracteres.")]
        [Display(Name = "curp")]
        [JsonPropertyName("curp")]
        public string Curp { get; set; } = null!;


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "nombre")]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "primer_apellido")]
        [JsonPropertyName("primer_apellido")]
        public string? PrimerApellido { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "segundo_apellido")]
        [JsonPropertyName("segundo_apellido")]
        public string? SegundoApellido { get; set; }


        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo '{0}' debe tener formato YYYY-MM-DD.")]
        [Display(Name = "fecha_nacimiento")]
        [JsonPropertyName("fecha_nacimiento")]
        public string? FechaNacimiento { get; set; }


        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo '{0}' debe tener formato YYYY-MM-DD.")]
        [Display(Name = "fecha_desaparicion")]
        [JsonPropertyName("fecha_desaparicion")]
        public string? FechaDesaparicion { get; set; }


        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(20, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "lugar_nacimiento")]
        [JsonPropertyName("lugar_nacimiento")]
        public string LugarNacimiento { get; set; } = null!;


        [StringLength(1, ErrorMessage = "El campo '{0}' debe contener solo un carácter.")]
        [RegularExpression("^(H|M|X)$", ErrorMessage = "El campo '{0}' debe ser H, M o X.")]
        [Display(Name = "sexo_asignado")]
        [JsonPropertyName("sexo_asignado")]
        public string? SexoAsignado { get; set; }


        [StringLength(15, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "telefono")]
        [JsonPropertyName("telefono")]
        public string? Telefono { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [EmailAddress(ErrorMessage = "El campo '{0}' debe ser un correo válido.")]
        [Display(Name = "correo")]
        [JsonPropertyName("correo")]
        public string? Correo { get; set; }


        [StringLength(500, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "direccion")]
        [JsonPropertyName("direccion")]
        public string? Direccion { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "calle")]
        [JsonPropertyName("calle")]
        public string? Calle { get; set; }


        [StringLength(20, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "numero")]
        [JsonPropertyName("numero")]
        public string? Numero { get; set; }


        [StringLength(50, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "colonia")]
        [JsonPropertyName("colonia")]
        public string? Colonia { get; set; }


        [StringLength(5, MinimumLength = 5, ErrorMessage = "El campo '{0}' debe tener {1} caracteres.")]
        [Display(Name = "codigo_postal")]
        [JsonPropertyName("codigo_postal")]
        public string? CodigoPostal { get; set; }


        [StringLength(100, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "municipio_o_alcaldia")]
        [JsonPropertyName("municipio_o_alcaldia")]
        public string? MunicipioOAlcaldia { get; set; }


        [StringLength(40, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = "entidad_federativa")]
        [JsonPropertyName("entidad_federativa")]
        public string? EntidadFederativa { get; set; }
    }
}