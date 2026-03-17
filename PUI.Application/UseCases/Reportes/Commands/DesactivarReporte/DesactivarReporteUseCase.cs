using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Utils.Mediator;
using PUI.Domain.Entities;
using PUI.Domain.Exceptions;


namespace PUI.Application.UseCases.Reportes.Commands.DesactivarReporte
{
    public class DesactivarReporteUseCase : IRequestHandler<DesactivarReporteCommand, Guid>
    {
        private readonly IReportesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportesHistorialRepository _historialRepository;
        private readonly IEventosRepository _eventosRepository;

        public DesactivarReporteUseCase(
            IReportesRepository repository,
            IUnitOfWork unitOfWork,
            IReportesHistorialRepository historialRepository,
            IEventosRepository eventosRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _historialRepository = historialRepository;
            _eventosRepository = eventosRepository;
        }

        public async Task<Guid> Handle(DesactivarReporteCommand request)
        {
            try
            {
                // 1. Buscar reporte existente
                var reporte = await _repository.ObtenerPorFolioPUI(request.Id);

                if (reporte == null)
                    throw new ExcepcionNoEncontrado();

                var estatusAnterior = reporte.Estatus;

                // 2. Desactivar (usar dominio)
                reporte.FinalizarReporte(DateTime.UtcNow);

                // 3. Historial
                var historial = new ReporteHistorial(
                    idReporte: reporte.Id,
                    estatusNuevo: reporte.Estatus,
                    estatusAnterior: estatusAnterior,
                    motivo: "Desactivación desde PUI"
                );

                await _historialRepository.Agregar(historial);

                // 4. Evento
                var evento = new Evento(
                    tipoEvento: "DESACTIVACION",
                    origen: "API_PUI",
                    idReporte: reporte.Id,
                    curp: reporte.Curp,
                    resultado: "EXITO",
                    descripcion: "Reporte desactivado correctamente"
                );

                await _eventosRepository.Agregar(evento);

                // 5. Persistir
                await _unitOfWork.Persistir();

                return reporte.Id;
            }
            catch
            {
                await _unitOfWork.Reversar();
                throw;
            }
        }


    }
}