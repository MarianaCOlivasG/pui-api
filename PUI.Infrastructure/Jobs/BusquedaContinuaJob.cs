using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Interfaces.Servicios;
using PUI.Domain.Entities;

namespace PUI.Infrastructure.Jobs
{
    public class BusquedaContinuaJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BusquedaContinuaJob> _logger;

        private readonly int _horaEjecucion;
        private readonly int _minutoEjecucion;
        private readonly int _delayHoras;
        private readonly int _delayMinutos;

        public BusquedaContinuaJob(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<BusquedaContinuaJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            _horaEjecucion = configuration.GetValue<int>("Jobs:BusquedaContinua:HoraEjecucion");
            _minutoEjecucion = configuration.GetValue<int>("Jobs:BusquedaContinua:MinutoEjecucion");

            _delayHoras = configuration.GetValue<int>("Jobs:BusquedaContinua:DelayHorasEjecucion");
            _delayMinutos = configuration.GetValue<int>("Jobs:BusquedaContinua:DelayMinutosEjecucion");

            if (_delayHoras == 0 && _delayMinutos == 0)
                _delayMinutos = 1;

            _logger.LogInformation(
                "Job configurado -> Hora: {hora}:{minuto}, Delay: {horas}h {min}m",
                _horaEjecucion,
                _minutoEjecucion,
                _delayHoras,
                _delayMinutos);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BusquedaContinuaJob iniciado");

            await EsperarProximaEjecucion(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await EjecutarProceso(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error general en el job");
                }

                var delay = TimeSpan.FromHours(_delayHoras)
                           + TimeSpan.FromMinutes(_delayMinutos);

                _logger.LogInformation(
                    "Esperando {horas}h {min}m para siguiente ejecución",
                    _delayHoras,
                    _delayMinutos);

                await Task.Delay(delay, stoppingToken);
            }
        }

        private async Task EsperarProximaEjecucion(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

            var ahoraLocal = time.NowLocal;

            var proxima = new DateTime(
                ahoraLocal.Year,
                ahoraLocal.Month,
                ahoraLocal.Day,
                _horaEjecucion,
                _minutoEjecucion,
                0);

            if (ahoraLocal >= proxima)
                proxima = proxima.AddDays(1);

            var delay = proxima - ahoraLocal;

            _logger.LogInformation(
                "Primera ejecución en: {fecha} (en {minutos} minutos)",
                proxima,
                delay.TotalMinutes);

            await Task.Delay(delay, stoppingToken);
        }

        private async Task EjecutarProceso(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var reportesRepo = scope.ServiceProvider.GetRequiredService<IReportesRepository>();
            var procesosRepo = scope.ServiceProvider.GetRequiredService<IProcesoBusquedaRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

            var proceso = ProcesoBusqueda.CrearContinua();

            try
            {
                // 1. Guardar inicio del proceso
                await procesosRepo.Agregar(proceso);
                await unitOfWork.Persistir();

                var ahoraUtc = time.UtcNow;

                // 2. Obtener reportes
                var reportes = await reportesRepo.ObtenerParaBusquedaContinua(60);

                if (!reportes.Any())
                {
                    _logger.LogInformation("No hay reportes pendientes");

                    proceso.Completar();
                    await unitOfWork.Persistir();
                    return;
                }

                _logger.LogInformation("Procesando {count} reportes", reportes.Count);

                foreach (var reporte in reportes)
                {
                    try
                    {
                        var fechaInicio = reporte.FechaUltimaBusqueda ?? reporte.FechaActivacion;

                        // NORMALIZAR A UTC
                        if (fechaInicio.Kind == DateTimeKind.Unspecified)
                        {
                            fechaInicio = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
                        }

                        var fechaFin = ahoraUtc;

                        // SOLO PARA LOG (hora Cancún correcta)
                        var inicioLocal = time.ConvertToLocal(fechaInicio);
                        var finLocal = time.ConvertToLocal(fechaFin);

                        _logger.LogInformation(
                            "Procesando reporte {id} desde {inicio} hasta {fin}",
                            reporte.Id,
                            inicioLocal,
                            finLocal);

                        // Simulación
                        await Task.Delay(10, stoppingToken);

                        proceso.IncrementarEvaluados();

                        // Control en UTC
                        reporte.ActualizarFechaUltimaBusqueda(fechaFin);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando reporte {id}", reporte.Id);
                    }
                }

                proceso.Completar();
                await unitOfWork.Persistir();

                _logger.LogInformation(
                    "Proceso completado. Evaluados: {eval}",
                    proceso.RegistrosEvaluados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico en ejecución del proceso");

                try
                {
                    proceso.MarcarError(ex.ToString());
                    await unitOfWork.Persistir();
                }
                catch (Exception innerEx)
                {
                    _logger.LogError(innerEx, "Error guardando estado de fallo del proceso");
                }
            }
        }
    }
}