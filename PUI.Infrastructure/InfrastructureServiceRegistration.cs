
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PUI.Application.Interfaces.ApiPUI;
using PUI.Application.Interfaces.Busqueda;
using PUI.Application.Interfaces.Servicios;
using PUI.Infrastructure.ApiPUI;
using PUI.Infrastructure.Services;

namespace DientesLimpios.Persistencia
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AgregarServiciosDeInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IBusquedaService, BusquedaService>();

            services.Configure<ApiPuiSettings>(configuration.GetSection("ApiPui"));

            services.AddHttpClient<IApiPuiService, ApiPuiService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ApiPuiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            return services;
        }
    }
}
