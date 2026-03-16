using System.ComponentModel.DataAnnotations;

namespace PUI.Api.DTOs.Reportes
{
    public class CrearReporteDto
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(254, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = nameof(FolioPui))]
        public string FolioPui { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(150, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = nameof(Nombre))]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(150, ErrorMessage = "El campo '{0}' no puede exceder los {1} caracteres.")]
        [Display(Name = nameof(PrimerApellido))]
        public string PrimerApellido { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(18, ErrorMessage = "El campo '{0}' debe tener máximo {1} caracteres.")]
        [Display(Name = nameof(Curp))]
        public string Curp { get; set; } = null!;

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(1, ErrorMessage = "El campo '{0}' debe contener {1} carácter.")]
        [Display(Name = nameof(Sexo))]
        public string Sexo { get; set; } = null!;
    }
}