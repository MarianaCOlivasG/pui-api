

namespace PUI.Application.UseCases.Reportes.Dtos
{
    public class ReporteListadoDto
    {
        public Guid Id { get; set; }

        public string FolioPui { get; set; } = null!;
        public string Curp { get; set; } = null!;

        public string NombreCompleto { get; set; } = null!;

        public DateTime FechaActivacion { get; set; }

        public string Estatus { get; set; } = null!;
    }
}