
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PUI.Application.Interfaces.ApiPUI;
using PUI.Application.Interfaces.Servicios;
using PUI.Application.UseCases;
using PUI.Infrastructure.ApiPUI;
using PUI.Infrastructure.Services;

namespace PUI.Persistencia
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AgregarServiciosDeInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.Configure<ApiPuiSettings>(configuration.GetSection("ApiPui"));

            services.Configure<ApiPuiSettings>(configuration.GetSection("ApiPui"));

            services.AddHttpClient<IApiPuiService, ApiPuiService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ApiPuiSettings>>().Value;

                if (string.IsNullOrEmpty(settings.BaseUrl)) throw new Exception("ApiPui:BaseUrl no está configurada");

                client.BaseAddress = new Uri(settings.BaseUrl);
            });


            return services;
        }
    }
}
