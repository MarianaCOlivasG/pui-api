using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Persistence.Repositories;
using PUI.Persistence.UnitOfWorks;

namespace PUI.Persistencia
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AgregarServiciosDePersistencia(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PuiConnectionString");

            services.AddDbContext<PUIDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            services.AddScoped<IReportesRepository, ReportesRepository>();
            services.AddScoped<IReportesHistorialRepository, ReportesHistorialRepository>();
            services.AddScoped<IEventosRepository, EventosRepository>();
            services.AddScoped<ICoincidenciasRepository, CoincidenciasRepository>();
            services.AddScoped<IProcesoBusquedaRepository, ProcesoBusquedaRepository>();
            services.AddScoped<INotificacionesPamRepository, NotificacionesPamRepository>();
            services.AddScoped<IApiLogsRepository, ApiLogsRepository>();

            services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();

            return services;
        }
    }
}