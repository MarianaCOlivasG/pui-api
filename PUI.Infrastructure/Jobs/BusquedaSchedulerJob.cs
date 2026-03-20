

using Hangfire;
using PUI.Application.Interfaces.Jobs;
using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace PUI.Infrastructure.Jobs
{
    public class BusquedaSchedulerJob : IBusquedaSchedulerJob
    {
        private readonly IBackgroundJobClient _client;
        private readonly IReportesRepository _reportesRepository;

        public BusquedaSchedulerJob(
            IBackgroundJobClient client,
            IReportesRepository reportesRepository)
        {
            _client = client;
            _reportesRepository = reportesRepository;
        }

        public async Task EjecutarBatch()
        {
            Console.WriteLine($"[{DateTime.UtcNow}] *** EjecutarBatch iniciado ***");

            var reportes = await _reportesRepository.Query()
                .AsNoTracking()
                .Where(r =>
                    r.Estatus == EstatusReporte.Activo &&
                    r.FechaDesactivacion == null &&
                    (r.FechaUltimaBusqueda == null ||
                     r.FechaUltimaBusqueda <= DateTime.UtcNow.AddMinutes(-60))
                )
                .OrderBy(r => r.FechaUltimaBusqueda)
                .Take(20)
                .ToListAsync();

            Console.WriteLine($"[{DateTime.UtcNow}] Reportes encontrados: {reportes.Count}");

            foreach (var reporte in reportes)
            {
                _client.Enqueue<IBusquedaFase3Job>(x => x.Ejecutar(reporte.Id));
            }

            Console.WriteLine($"[{DateTime.UtcNow}] *** EjecutarBatch terminado ***");
        }

    }
}
