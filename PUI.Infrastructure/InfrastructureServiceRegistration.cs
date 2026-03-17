
using Microsoft.Extensions.DependencyInjection;
using PUI.Application.Interfaces.Busqueda;
using PUI.Infrastructure.Services;

namespace DientesLimpios.Persistencia
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AgregarServiciosDeInfraestructure(this IServiceCollection services)
        {


            services.AddScoped<IBusquedaService, BusquedaService>();

            return services;
        }
    }
}
