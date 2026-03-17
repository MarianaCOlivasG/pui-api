
using Microsoft.Extensions.DependencyInjection;
using PUI.Application.Interfaces.Busqueda;
using PUI.Application.Interfaces.Servicios;
using PUI.Infrastructure.Services;

namespace DientesLimpios.Persistencia
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AgregarServiciosDeInfraestructure(this IServiceCollection services)
        {

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IBusquedaService, BusquedaService>();


            return services;
        }
    }
}
