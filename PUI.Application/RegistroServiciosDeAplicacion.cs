


using PUI.Application.Utils.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace PUI.Application
{
    
    public static class RegistroServiciosDeAplicacion
    {
        
        public static IServiceCollection AgregarServiciosDeAplicacion( this IServiceCollection services )
        {
            services.AddScoped<IMediator, SimpleMediator>();

            services.Scan(
                scan => scan.FromAssembliesOf( typeof(IMediator) )
                    .AddClasses( c => c.AssignableTo( typeof(IRequestHandler<,>) ) )
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()

                    .AddClasses( c => c.AssignableTo( typeof(IRequestHandler<>) ) )
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );


            return services;
        }


    }


}