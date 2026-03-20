using Hangfire;
using PUI.Application.Interfaces.Busqueda;
using PUI.Application.UseCases.Reportes.Orquestacion;

namespace PUI.Infrastructure.Busqueda
{
    public class OrquestadorReportes : IOrquestadorReportes
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public OrquestadorReportes(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public Task IniciarFlujoAsync(Guid reporteId)
        {
            _backgroundJobClient.Enqueue<IBusquedaFase1Service>(
                x => x.Ejecutar(reporteId)
            );

            _backgroundJobClient.Schedule<IBusquedaFase2Service>(
                x => x.Ejecutar(reporteId),
                TimeSpan.FromMinutes(2)
            );

            return Task.CompletedTask;
        }
    }
}