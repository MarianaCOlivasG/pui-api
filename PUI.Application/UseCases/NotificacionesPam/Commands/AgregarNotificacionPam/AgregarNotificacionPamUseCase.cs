

using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Utils.Mediator;
using PUI.Domain.Entities;
using PUI.Domain.ValueObjects;

namespace PUI.Application.UseCases.NotificacionesPam.Commands.AgregarNotificacionPam
{
    public class AgregarNotificacionPamUseCase : IRequestHandler<AgregarNotificacionPamCommand, Guid>
    {
        private readonly INotificacionesPamRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AgregarNotificacionPamUseCase(
            INotificacionesPamRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AgregarNotificacionPamCommand request)
        {
            try
            {
                // Agregar la notificación en basde de datos
                var notificacionPam = new NotificacionPam(
                    curp: new Curp(request.Curp),
                    nombre: request.Nombre,
                    primerApellido: request.PrimerApellido,
                    segundoApellido: request.SegundoApellido,
                    tipoEvento: "PRUEBA",
                    fechaEvento: DateTime.UtcNow,
                    descripcionLugarEvento: "PRUEBA"
                 );

                var respuesta = await _repository.Agregar(notificacionPam);

                // Realizar busqueda inicial, para completar datos

                // Realizar busqueda historica

                // Notificar PUI

                await _unitOfWork.Persistir();

                return respuesta.Id;
            }
            catch
            {
                await _unitOfWork.Reversar();
                throw;
            }
        }

    }
}
