using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Reportes.Commands.DesactivarReporte
{
    public class DesactivarReporteCommand : IRequest<Guid>
    {
        public required string Id { get; set; }
    }
}