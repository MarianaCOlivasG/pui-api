using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Interfaces.Servicios;
using PUI.Domain.Enums;
using PUI.Domain.ValueObjects;

namespace PUI.Infrastructure.Jobs
{
    public class BusquedaContinuaOptimizadaJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BusquedaContinuaOptimizadaJob> _logger;

        private readonly int _horaEjecucion;
        private readonly int _minutoEjecucion;
        private readonly int _delayHoras;
        private readonly int _delayMinutos;

        public BusquedaContinuaOptimizadaJob(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<BusquedaContinuaOptimizadaJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            var config = configuration.GetSection("Jobs:BusquedaContinua");

            _horaEjecucion = config.GetValue<int>("HoraEjecucion");
            _minutoEjecucion = config.GetValue<int>("MinutoEjecucion");
            _delayHoras = config.GetValue<int>("DelayHorasEjecucion");
            _delayMinutos = config.GetValue<int>("DelayMinutosEjecucion");

            if (_delayHoras == 0 && _delayMinutos == 0)
                _delayMinutos = 1;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Job BusquedaContinua iniciado");

            await EsperarPrimeraEjecucion(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await EjecutarProceso(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en BusquedaContinuaOptimizadaJob");
                }

                var delay = TimeSpan.FromHours(_delayHoras)
                           + TimeSpan.FromMinutes(_delayMinutos);

                await Task.Delay(delay, stoppingToken);
            }
        }

        // Espera hasta la primera ejecución programada del día
        private async Task EsperarPrimeraEjecucion(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
            var now = time.NowLocal;

            var nextRun = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                _horaEjecucion,
                _minutoEjecucion,
                0
            );

            if (now >= nextRun)
                nextRun = nextRun.AddDays(1);

            var delay = nextRun - now;

            _logger.LogInformation("Primera ejecución programada: {time}", nextRun);

            await Task.Delay(delay, stoppingToken);
        }

        private async Task EjecutarProceso(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var reportesRepo = scope.ServiceProvider.GetRequiredService<IReportesRepository>();
            var notificacionesRepo = scope.ServiceProvider.GetRequiredService<INotificacionesPamRepository>();
            var coincidenciasRepo = scope.ServiceProvider.GetRequiredService<ICoincidenciasRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var ahoraUtc = DateTime.UtcNow;

            // Limite de reintento: solo reportes que no se hayan procesado en los últimos 60 minutos
            var limiteReintento = ahoraUtc.AddMinutes(-60);

            // Obtener reportes filtrados por estado y ventana de búsqueda
            var reportes = await reportesRepo.Query()
                .AsNoTracking()
                .Where(r =>
                    r.Estatus == EstatusReporte.Activo &&
                    r.FechaDesactivacion == null &&
                    (
                        r.FechaUltimaBusqueda == null ||
                        r.FechaUltimaBusqueda <= limiteReintento
                    )
                )
                .OrderBy(r => r.FechaUltimaBusqueda)
                .Take(10)
                .ToListAsync(stoppingToken);

            if (!reportes.Any())
            {
                _logger.LogInformation("No hay reportes para procesar");
                return;
            }

            // Obtener CURPs únicas de los reportes
            var curps = reportes
                .Select(r => r.Curp.Valor)
                .Distinct()
                .ToList();

            // Obtener notificaciones PAM relacionadas a las CURPs
            var notificaciones = await notificacionesRepo.Query()
                .AsNoTracking()
                .Where(n => curps.Contains(n.Curp.Valor))
                .ToListAsync(stoppingToken);

            if (!notificaciones.Any())
            {
                _logger.LogInformation("No hay notificaciones PAM");
                return;
            }

            // Indexar notificaciones en memoria por CURP
            var notificacionesPorCurp = notificaciones
                .GroupBy(n => n.Curp.Valor)
                .ToDictionary(g => g.Key, g => g.ToList());

            int creadas = 0;

            // Procesamiento en paralelo por reporte
            await Parallel.ForEachAsync(reportes, new ParallelOptions
            {
                MaxDegreeOfParallelism = 4,
                CancellationToken = stoppingToken
            },
            async (reporte, ct) =>
            {
                using var innerScope = _scopeFactory.CreateScope();

                var repoCoincidencias = innerScope.ServiceProvider.GetRequiredService<ICoincidenciasRepository>();
                var repoReportes = innerScope.ServiceProvider.GetRequiredService<IReportesRepository>();
                var uow = innerScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                try
                {
                    if (!notificacionesPorCurp.TryGetValue(reporte.Curp.Valor, out var lista))
                        return;

                    foreach (var n in lista)
                    {
                        var folio = $"{reporte.FolioPui}-{n.Id}";

                        // Evitar duplicados (idempotencia)
                        var existe = await repoCoincidencias.Query()
                            .AsNoTracking()
                            .AnyAsync(c => c.FolioNotificacionPui == folio, ct);

                        if ( existe ) continue;

                        // Crear coincidencia
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

                        await repoCoincidencias.Agregar(coincidencia);
                        Interlocked.Increment(ref creadas);
                    }

                    // Actualizar última fecha de búsqueda del reporte
                    reporte.ActualizarFechaUltimaBusqueda(DateTime.UtcNow);

                    await repoReportes.Actualizar(reporte);
                    await uow.Persistir();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando reporte {id}", reporte.Id);
                }
            });

            // Persistencia final de seguridad
            await unitOfWork.Persistir();

            _logger.LogInformation("Coincidencias creadas: {count}", creadas);
        }
    }
}