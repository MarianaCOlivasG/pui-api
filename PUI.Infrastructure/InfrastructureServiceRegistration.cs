
using Microsoft.Extensions.DependencyInjection;

namespace PUI.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AgregarServiciosDeInfraestructure(this IServiceCollection services)
        {
            //services.AddScoped<IMensajesService, MensajesService>();

            return services;
        }
    }
}
