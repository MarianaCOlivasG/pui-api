using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;

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

            // 🔥 fallback inteligente
            if (_delayHoras == 0 && _delayMinutos == 0)
            {
                _delayMinutos = 1; // mínimo 1 minuto
            }

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
            var ahora = DateTime.Now;

            var proxima = new DateTime(
                ahora.Year,
                ahora.Month,
                ahora.Day,
                _horaEjecucion,
                _minutoEjecucion,
                0);

            if (ahora >= proxima)
            {
                proxima = proxima.AddDays(1);
            }

            var delay = proxima - ahora;

            _logger.LogInformation(
                "Primera ejecución en: {fecha} (en {minutos} minutos)",
                proxima,
                delay.TotalMinutes);

            await Task.Delay(delay, stoppingToken);
        }

        private async Task EjecutarProceso(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IReportesRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var ahora = DateTime.UtcNow;

            var reportes = await repository.ObtenerParaBusquedaContinua(60);

            if (!reportes.Any())
            {
                _logger.LogInformation("No hay reportes pendientes");
                return;
            }

            _logger.LogInformation("Procesando {count} reportes", reportes.Count);

            int registrosEvaluados = 0;

            foreach (var reporte in reportes)
            {
                try
                {
                    var fechaInicio = reporte.FechaUltimaBusqueda ?? reporte.FechaActivacion;
                    var fechaFin = ahora;

                    _logger.LogInformation(
                        "Procesando reporte {id} desde {inicio} hasta {fin}",
                        reporte.Id, fechaInicio, fechaFin);

                    await Task.Delay(10, stoppingToken);

                    reporte.ActualizarFechaUltimaBusqueda(fechaFin);

                    registrosEvaluados++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando reporte {id}", reporte.Id);
                }
            }

            await unitOfWork.Persistir();

            _logger.LogInformation(
                "Proceso completado. Evaluados: {eval}",
                registrosEvaluados);
        }
    }

}