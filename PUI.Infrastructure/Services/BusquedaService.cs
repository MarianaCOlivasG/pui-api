using PUI.Application.Interfaces.Busqueda;


namespace PUI.Infrastructure.Services
{
    public class BusquedaService : IBusquedaService
    {
        public Task IniciarBusqueda()
        {
           Console.WriteLine("INICIAR BUSQUEDA");

            return Task.CompletedTask;
        }
    }
}
