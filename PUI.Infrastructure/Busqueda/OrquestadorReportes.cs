using PUI.Application.Interfaces.Busqueda;
using PUI.Application.Interfaces.Servicios;
using PUI.Application.UseCases.Reportes.Orquestacion;

namespace PUI.Infrastructure.Busqueda
{


    public class OrquestadorReportes : IOrquestadorReportes
    {
        private readonly IBusquedaFase1Service _fase1;
        private readonly IBusquedaFase2Service _fase2;

        public OrquestadorReportes(
            IBusquedaFase1Service fase1,
            IBusquedaFase2Service fase2)
        {
            _fase1 = fase1;
            _fase2 = fase2;
        }

        public Task IniciarFlujoAsync(Guid reporteId)
        {
            // FIRE AND FORGET (NO BLOQUEA API)
            _ = Task.Run(async () =>
            {
                await _fase1.Ejecutar(reporteId);

                await Task.Delay(TimeSpan.FromMinutes(2));

                await _fase2.Ejecutar(reporteId);

                // fase 3 futura aquí
            });

            return Task.CompletedTask;
        }
    }


}