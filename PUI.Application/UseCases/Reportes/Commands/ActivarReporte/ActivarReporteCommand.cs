using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Reportes.Commands.ActivarReporte
{
    public class ActivarReporteCommand : IRequest<Guid>
    {
        public required string Id { get; set; }
        public required string Curp { get; set; }
        public required string LugarNacimiento { get; set; }

        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? FechaNacimiento { get; set; }
        public string? FechaDesaparicion { get; set; }
        public string? SexoAsignado { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Colonia { get; set; }
        public string? CodigoPostal { get; set; }
        public string? MunicipioOAlcaldia { get; set; }
        public string? EntidadFederativa { get; set; }
    }
}