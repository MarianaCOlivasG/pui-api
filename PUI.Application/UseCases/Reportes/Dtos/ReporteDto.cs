namespace PUI.Application.UseCases.Reportes.Dtos
{
    public class ReporteDto
    {
        public Guid Id { get; set; }
        public string FolioPui { get; set; } = null!;
        public string? Curp { get; set; }

        public string Nombre { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaDesaparicion { get; set; }

        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        public DateTime FechaActivacion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
    }
}