
using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.NotificacionesPam.Commands.AgregarNotificacionPam
{
    public class AgregarNotificacionPamCommand : IRequest<Guid>
    {
        public required string Curp { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? TipoEvento { get; set; }
        public DateTime? FechaEvento { get; set; }
        public string? DescripcionLugarEvento { get; set; }
    }
}
