


using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PUI.Application.Utils.Mediator;

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

            services.AddValidatorsFromAssemblies(new[] { typeof(RegistroServiciosDeAplicacion).Assembly });



            return services;
        }


    }


}