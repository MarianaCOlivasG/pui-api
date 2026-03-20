using PUI.Application.Interfaces.Busqueda;

namespace PUI.Infrastructure.Busqueda
{
    public class BusquedaFase2Service : IBusquedaFase2Service
    {
        public async Task Ejecutar(Guid reporteId)
        {
            Console.WriteLine("Ejecutando Busqueda Fase 2 para reporteId: " + reporteId);
        }
    }
}
