using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PUI.Application.Interfaces.ApiPUI;
using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Interfaces.Servicios;
using PUI.Application.Utils.Paginacion;
using PUI.Domain.Entities;
using PUI.Domain.Enums;

namespace PUI.Infrastructure.Jobs
{
    // BackgroundService permite ejecutar procesos en segundo plano dentro de ASP.NET Core
    public class BusquedaContinuaJob : BackgroundService
    {
        // Factory para crear scopes (inyección de dependencias por ejecución)
        private readonly IServiceScopeFactory _scopeFactory;

        // Logger para registrar eventos del job
        private readonly ILogger<BusquedaContinuaJob> _logger;

        // Hora específica de primera ejecución
        private readonly int _horaEjecucion;
        private readonly int _minutoEjecucion;

        // Delay entre ejecuciones posteriores
        private readonly int _delayHoras;
        private readonly int _delayMinutos;

        // Constructor donde se leen configuraciones del appsettings
        public BusquedaContinuaJob(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<BusquedaContinuaJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            // Hora inicial del job
            _horaEjecucion = configuration.GetValue<int>("Jobs:BusquedaContinua:HoraEjecucion");
            _minutoEjecucion = configuration.GetValue<int>("Jobs:BusquedaContinua:MinutoEjecucion");

            // Delay entre ejecuciones
            _delayHoras = configuration.GetValue<int>("Jobs:BusquedaContinua:DelayHorasEjecucion");
            _delayMinutos = configuration.GetValue<int>("Jobs:BusquedaContinua:DelayMinutosEjecucion");

            // Si no hay delay configurado, usar 1 minuto como fallback
            if (_delayHoras == 0 && _delayMinutos == 0)
                _delayMinutos = 1;

            // Log de configuración inicial
            _logger.LogInformation(
                "Job configurado -> Hora: {hora}:{minuto}, Delay: {horas}h {min}m",
                _horaEjecucion,
                _minutoEjecucion,
                _delayHoras,
                _delayMinutos);
        }

        // Método principal que se ejecuta al iniciar el servicio
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BusquedaContinuaJob iniciado");

            // Espera hasta la hora configurada antes de iniciar
            await EsperarProximaEjecucion(stoppingToken);

            // Loop infinito hasta que el servicio se detenga
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Ejecuta el proceso principal
                    await EjecutarProceso(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Se cancela el proceso si se detiene el servicio
                    _logger.LogWarning("Ejecución cancelada");
                }
                catch (Exception ex)
                {
                    // Manejo de errores generales
                    _logger.LogError(ex, "Error general en el job");
                }

                // Calcula el delay entre ejecuciones
                var delay = TimeSpan.FromHours(_delayHoras)
                           + TimeSpan.FromMinutes(_delayMinutos);

                _logger.LogInformation(
                    "Esperando {horas}h {min}m para siguiente ejecución",
                    _delayHoras,
                    _delayMinutos);

                // Espera antes de la siguiente ejecución
                await Task.Delay(delay, stoppingToken);
            }
        }

        // Espera hasta la siguiente ejecución programada (hora específica del día)
        private async Task EsperarProximaEjecucion(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            // Servicio para manejar fechas (importante para pruebas y consistencia)
            var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

            var ahoraLocal = time.NowLocal;

            // Se construye la próxima fecha de ejecución
            var proxima = new DateTime(
                ahoraLocal.Year,
                ahoraLocal.Month,
                ahoraLocal.Day,
                _horaEjecucion,
                _minutoEjecucion,
                0);

            // Si ya pasó la hora, se programa para el siguiente día
            if (ahoraLocal >= proxima)
                proxima = proxima.AddDays(1);

            // Calcula cuánto tiempo falta
            var delay = proxima - ahoraLocal;

            _logger.LogInformation(
                "Primera ejecución en: {fecha} (en {minutos} minutos)",
                proxima,
                delay.TotalMinutes);

            // Espera hasta la hora programada
            await Task.Delay(delay, stoppingToken);
        }

        // Proceso principal del job
        private async Task EjecutarProceso(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            // Se obtienen dependencias
            var reportesRepo = scope.ServiceProvider.GetRequiredService<IReportesRepository>();
            var procesosRepo = scope.ServiceProvider.GetRequiredService<IProcesoBusquedaRepository>();
            var notificacionesPamRepo = scope.ServiceProvider.GetRequiredService<INotificacionesPamRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
            var apiPui = scope.ServiceProvider.GetRequiredService<IApiPuiService>();

            // Se crea un nuevo proceso de búsqueda
            var proceso = ProcesoBusqueda.CrearContinua();

            try
            {
                // Se guarda el inicio del proceso
                await procesosRepo.Agregar(proceso);
                await unitOfWork.Persistir();

                // Tiempo actual en UTC (referencia única para todo el proceso)
                var ahoraUtc = time.UtcNow;

                // Se obtienen reportes que necesitan búsqueda
                var reportes = await reportesRepo.ObtenerParaBusquedaContinua(60);

                // Si no hay reportes, se termina el proceso
                if (!reportes.Any())
                {
                    _logger.LogInformation("No hay reportes pendientes");

                    proceso.Completar();
                    await unitOfWork.Persistir();
                    return;
                }

                _logger.LogInformation("Procesando {count} reportes", reportes.Count);

                // Itera cada reporte
                foreach (var reporte in reportes)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    try
                    {
                        // Solo procesa reportes activos
                        if (reporte.Estatus != EstatusReporte.Activo)
                        {
                            _logger.LogInformation(
                                "Reporte {id} omitido porque no está activo",
                                reporte.Id);
                            continue;
                        }

                        // Ejecuta la búsqueda para ese reporte
                        await ProcesarBusquedaAsync(
                            reporte,
                            apiPui,
                            notificacionesPamRepo,
                            ahoraUtc,
                            stoppingToken);

                        // Incrementa contador de evaluados
                        proceso.IncrementarEvaluados();

                        // Actualiza última búsqueda
                        reporte.ActualizarFechaUltimaBusqueda(ahoraUtc);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando reporte {id}", reporte.Id);
                    }
                }

                // Marca proceso como completado
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

        // Método que realiza la búsqueda de coincidencias
        private async Task ProcesarBusquedaAsync(
            Reporte reporte,
            IApiPuiService apiPui,
            INotificacionesPamRepository notificacionesPamRepo,
            DateTime ahoraUtc,
            CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reporte con CURP: {curp}", reporte.Curp.Valor);

            // Determina fecha inicio de búsqueda
            var fechaInicio = reporte.FechaUltimaBusqueda
                ?? reporte.FechaDesaparicion
                ?? reporte.FechaActivacion;

            // Normaliza fecha a UTC si viene sin tipo
            if (fechaInicio.Kind == DateTimeKind.Unspecified)
                fechaInicio = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);

            // Limita búsqueda a máximo 30 días atrás para evitar consultas grandes
            var limiteMaximo = ahoraUtc.AddDays(-30);

            if (fechaInicio < limiteMaximo)
                fechaInicio = limiteMaximo;

            // Query filtrando por CURP y rango de fechas
            var query = notificacionesPamRepo.Query()
                .Where(n =>
                    n.Curp.Valor == reporte.Curp.Valor &&
                    n.FechaEvento != null &&
                    n.FechaEvento >= fechaInicio &&
                    n.FechaEvento <= ahoraUtc
                );

            int pagina = 1;
            const int pageSize = 500;

            while (true)
            {
                var resultado = await notificacionesPamRepo
                    .ObtenerPaginado(query, new PaginacionDto(pagina, pageSize));

                if (!resultado.Registros.Any())
                    break;

                foreach (var notificacion in resultado.Registros)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    _logger.LogInformation(
                        "Coincidencia encontrada: {id}",
                        notificacion.Id);

                    // Aquí iría la lógica real de notificación
                    // await apiPui.NotificarCoincidencia(...)
                }

                // Si ya no hay más páginas, termina
                if (resultado.Registros.Count < pageSize)
                    break;

                pagina++;
            }
        }
    }
}