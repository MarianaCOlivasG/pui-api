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
        private readonly IReportesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportesHistorialRepository _historialRepository;
        private readonly IEventosRepository _eventosRepository;

        public ActivarReporteUseCase(
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

        public async Task<Guid> Handle(ActivarReporteCommand request)
        {
            try
            {
                // 1️ Crear reporte
                var reporte = CrearReporte(request);

                var respuesta = await _repository.Agregar(reporte);

                // 2️ Guardar historial
                var historial = new ReporteHistorial(
                    idReporte: respuesta.Id,
                    estatusNuevo: respuesta.Estatus,
                    estatusAnterior: null,
                    motivo: "Activación inicial desde PUI"
                );
                await _historialRepository.Agregar(historial);

                // 3️ Crear evento de negocio
                var evento = new Evento(
                    tipoEvento: "ACTIVACION",
                    origen: "API_PUI",
                    idReporte: respuesta.Id,
                    curp: respuesta.Curp,
                    resultado: "EXITO",
                    descripcion: "Reporte activado correctamente"
                );
                await _eventosRepository.Agregar(evento);

                // 4️ Persistir todo en una sola transacción
                await _unitOfWork.Persistir();

                return respuesta.Id;
            }
            catch
            {
                await _unitOfWork.Reversar();
                throw;
            }
        }

        private Reporte CrearReporte(ActivarReporteCommand request)
        {
            return new Reporte(
                folioPui: request.Id,
                curp: new Curp(request.Curp),
                lugarNacimiento: request.LugarNacimiento,
                nombre: request.Nombre,
                primerApellido: request.PrimerApellido,
                segundoApellido: request.SegundoApellido,
                sexo: string.IsNullOrWhiteSpace(request.SexoAsignado) ? null : Enum.Parse<Sexo>(request.SexoAsignado),
                fechaNacimiento: string.IsNullOrWhiteSpace(request.FechaNacimiento) ? null : DateTime.Parse(request.FechaNacimiento),
                fechaDesaparicion: string.IsNullOrWhiteSpace(request.FechaDesaparicion) ? null : DateTime.Parse(request.FechaDesaparicion),
                correo: string.IsNullOrWhiteSpace(request.Correo) ? null : new CorreoElectronico(request.Correo),
                telefono: request.Telefono,
                direccion: request.Direccion,
                calle: request.Calle,
                numero: request.Numero,
                colonia: request.Colonia,
                codigoPostal: request.CodigoPostal,
                municipioOAlcaldia: request.MunicipioOAlcaldia,
                entidadFederativa: request.EntidadFederativa
            );
        }

    }
}