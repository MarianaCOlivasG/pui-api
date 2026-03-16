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
                var curp = new Curp(request.Curp);

                CorreoElectronico? correo = null;
                if (!string.IsNullOrWhiteSpace(request.Correo))
                    correo = new CorreoElectronico(request.Correo);

                Sexo? sexo = null;
                if (!string.IsNullOrWhiteSpace(request.SexoAsignado))
                    sexo = Enum.Parse<Sexo>(request.SexoAsignado);

                DateTime? fechaNacimiento = null;
                if (!string.IsNullOrWhiteSpace(request.FechaNacimiento))
                    fechaNacimiento = DateTime.Parse(request.FechaNacimiento);

                DateTime? fechaDesaparicion = null;
                if (!string.IsNullOrWhiteSpace(request.FechaDesaparicion))
                    fechaDesaparicion = DateTime.Parse(request.FechaDesaparicion);

                var reporte = new Reporte(
                    request.Id,
                    curp,
                    request.LugarNacimiento,
                    request.Nombre,
                    request.PrimerApellido,
                    request.SegundoApellido,
                    sexo,
                    fechaNacimiento,
                    fechaDesaparicion,
                    correo,
                    request.Telefono
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