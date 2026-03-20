using Hangfire;
using Microsoft.Extensions.Hosting;
using PUI.Application.Interfaces.Jobs;

namespace PUI.Infrastructure.Jobs
{
    public class BusquedaSchedulerInitializer : IHostedService
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public BusquedaSchedulerInitializer(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _recurringJobManager.AddOrUpdate<IBusquedaSchedulerJob>(
                "busqueda-continua-pui",
                x => x.EjecutarBatch(),
                "*/3 * * * *" //Cada 3 minutos, para pruebas. Cambiar a "0 * * * *" para cada hora
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}