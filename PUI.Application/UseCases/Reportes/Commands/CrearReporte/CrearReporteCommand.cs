


using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Reportes.Commands.CrearReporte
{
    
    public class CrearReporteCommand: IRequest<Guid>
    {

        
        public required string FolioPui { get; set; }
        public required string Nombre { get; set; }

        public required string PrimerApellido { get; set; }

        public required string Curp { get; set; }

        public required string Sexo { get; set; }

    }

}