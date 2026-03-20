using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PUI.Application.Interfaces.Jobs;
using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Enums;
using PUI.Domain.ValueObjects;

namespace PUI.Infrastructure.Jobs
{
    public class BusquedaFase3Job : IBusquedaFase3Job
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BusquedaFase3Job> _logger;

        public BusquedaFase3Job(
            IServiceScopeFactory scopeFactory,
            ILogger<BusquedaFase3Job> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Ejecutar(Guid reporteId)
        {
            _logger.LogInformation("**** INICIO Fase3Job para reporte {ReporteId}", reporteId);

            using var scope = _scopeFactory.CreateScope();

            var reportesRepo = scope.ServiceProvider.GetRequiredService<IReportesRepository>();
            var notificacionesRepo = scope.ServiceProvider.GetRequiredService<INotificacionesPamRepository>();
            var coincidenciasRepo = scope.ServiceProvider.GetRequiredService<ICoincidenciasRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                _logger.LogInformation("**** Buscando reporte {ReporteId}", reporteId);

                var reporte = await reportesRepo.Query()
                    .FirstOrDefaultAsync(r => r.Id == reporteId);

                if (reporte == null)
                {
                    _logger.LogWarning("**** No se encontró el reporte {ReporteId}", reporteId);
                    return;
                }

                _logger.LogInformation("**** Buscando notificaciones para CURP {Curp}", reporte.Curp.Valor);

                var notificaciones = await notificacionesRepo.Query()
                    .Where(n => n.Curp.Valor == reporte.Curp.Valor)
                    .ToListAsync();

                if (!notificaciones.Any())
                {
                    _logger.LogInformation("**** Sin notificaciones para reporte {ReporteId}", reporteId);
                    return;
                }

                _logger.LogInformation("**** {Count} notificaciones encontradas para reporte {ReporteId}",
                    notificaciones.Count, reporteId);

                int creadas = 0;
                int saltadas = 0;

                foreach (var n in notificaciones)
                {
                    var folio = $"{reporte.FolioPui}-{n.Id}";

                    var existe = await coincidenciasRepo.Query()
                        .AnyAsync(c => c.FolioNotificacionPui == folio);

                    if (existe)
                    {
                        saltadas++;
                        _logger.LogDebug("**** Coincidencia ya existe para folio {Folio}", folio);
                        continue;
                    }

                    _logger.LogInformation("**** Creando coincidencia para folio {Folio}", folio);

                    var coincidencia = Coincidencia.Crear(
                        idReporte: reporte.Id,
                        folioNotificacionPui: folio,
                        curpEncontrada: new Curp(reporte.Curp.Valor),
                        tipoEvento: n.TipoEvento,
                        fechaEvento: n.FechaEvento,
                        descripcionLugarEvento: n.DescripcionLugarEvento,
                        detalleJson: null,
                        lugarNacimiento: reporte.LugarNacimiento,
                        guestId: "SYSTEM",
                        nivel: NivelCoincidencia.EXACT_CURP,
                        fase: FaseBusqueda.Fase3
                    );

                    await coincidenciasRepo.Agregar(coincidencia);
                    creadas++;
                }

                _logger.LogInformation(
                    "**** Resumen Fase3 -> creadas: {Creadas}, saltadas: {Saltadas}",
                    creadas, saltadas
                );

                reporte.ActualizarFechaUltimaBusqueda(DateTime.UtcNow);

                await reportesRepo.Actualizar(reporte);
                await unitOfWork.Persistir();

                _logger.LogInformation("**** Cambios persistidos para reporte {ReporteId}", reporteId);
                _logger.LogInformation("**** Fase3 COMPLETADA para reporte {ReporteId}", reporteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "**** ERROR en Fase3Job {ReporteId}", reporteId);
                throw;
            }
        }
    }
}