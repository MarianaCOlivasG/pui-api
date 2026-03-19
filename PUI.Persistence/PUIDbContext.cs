using PUI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PUI.Persistencia
{
    public class PUIDbContext : DbContext
    {
        public PUIDbContext(DbContextOptions<PUIDbContext> options) : base(options)
        {
        }

        protected PUIDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PUIDbContext).Assembly);
        }

        public DbSet<Reporte> Reportes { get; set; } = null!;
        public DbSet<ReporteHistorial> ReportesHistorial { get; set; } = null!;
        public DbSet<Coincidencia> Coincidencias { get; set; } = null!;
        public DbSet<Evento> Eventos { get; set; } = null!;
        public DbSet<ApiLog> ApiLogs { get; set; } = null!;
        public DbSet<ProcesoBusqueda> ProcesosBusqueda { get; set; } = null!;
        public DbSet<NotificacionPam> NotificacionesPam { get; set; } = null!;
    }
}