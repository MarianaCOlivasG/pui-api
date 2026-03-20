

using PUI.Application.Interfaces.Busqueda;

namespace PUI.Infrastructure.Busqueda
{
    public class BusquedaFase1Service : IBusquedaFase1Service
    {
        public async Task Ejecutar(Guid reporteId)
        {
            // Obtener la coincidencia
            // Notificar a pui 
            Console.WriteLine("Ejecutando Busqueda Fase 1 para reporteId: " + reporteId);
        }
    }
}
