

using PUI.Domain.ValueObjects;

namespace PUI.Application.UseCases.NotificacionesPam.Dtos
{
    public class NotificacionPamListadoDto
    {
        public Guid Id { get; set; }
        public string Curp { get; set; } = null!;
        public string? Nombre { get; set; } = null!;
        public string? PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; } = null!;
        public string? TipoEvento { get; set; } = null!;
        public DateTime? FechaEvento { get; set; } = null!;
        public string? DescripcionLugarEvento { get; set; } = null!;

    }
}
