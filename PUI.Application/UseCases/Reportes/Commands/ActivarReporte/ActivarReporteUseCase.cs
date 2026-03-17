using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Utils.Mediator;
using PUI.Domain.Entities;
using PUI.Domain.Enums;
using PUI.Domain.ValueObjects;

namespace PUI.Application.UseCases.Reportes.Commands.ActivarReporte
{
    public class ActivarReporteUseCase : IRequestHandler<ActivarReporteCommand, Guid>
    {
        private readonly IReportesRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public ActivarReporteUseCase(
            IReportesRepository repository,
            IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(ActivarReporteCommand request)
        {
            try
            {
                var reporte = new Reporte(
                    request.Id,
                    new Curp(request.Curp),
                    request.LugarNacimiento,
                    request.Nombre,
                    request.PrimerApellido,
                    request.SegundoApellido,
                    string.IsNullOrWhiteSpace(request.SexoAsignado) ? null : Enum.Parse<Sexo>(request.SexoAsignado),
                    string.IsNullOrWhiteSpace(request.FechaNacimiento) ? null : DateTime.Parse(request.FechaNacimiento),
                    string.IsNullOrWhiteSpace(request.FechaDesaparicion) ? null : DateTime.Parse(request.FechaDesaparicion),
                    string.IsNullOrWhiteSpace(request.Correo) ? null : new CorreoElectronico(request.Correo),
                    request.Telefono,
                    request.Direccion,
                    request.Calle,
                    request.Numero,
                    request.Colonia,
                    request.CodigoPostal,
                    request.MunicipioOAlcaldia,
                    request.EntidadFederativa
                );

                var respuesta = await repository.Agregar(reporte);
                await unitOfWork.Persistir();

                return respuesta.Id;
            }
            catch
            {
                await unitOfWork.Reversar();
                throw;
            }
        }

    }
}