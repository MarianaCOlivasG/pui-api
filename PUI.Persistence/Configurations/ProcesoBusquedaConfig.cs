
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;

namespace PUI.Persistence.Configurations
{
    public class ProcesoBusquedaConfig : IEntityTypeConfiguration<ProcesoBusqueda>
    {
        public void Configure(EntityTypeBuilder<ProcesoBusqueda> builder)
        {
            builder.ToTable("pui_procesos_busqueda");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id_proceso")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(p => p.IdReporte)
                .HasColumnName("id_reporte")
                .HasMaxLength(36);

            builder.Property(p => p.TipoBusqueda)
                .HasColumnName("tipo_busqueda")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.RegistrosEvaluados)
                .HasColumnName("registros_evaluados");

            builder.Property(p => p.CoincidenciasDetectadas)
                .HasColumnName("coincidencias_detectadas");

            builder.Property(p => p.FechaInicio)
                .HasColumnName("fecha_inicio");

            builder.Property(p => p.FechaFin)
                .HasColumnName("fecha_fin");

            builder.Property(p => p.Estatus)
                .HasColumnName("estatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.ErrorDetalle)
                .HasColumnName("error_detalle");

            builder.HasOne(p => p.Reporte)
                .WithMany()
                .HasForeignKey(p => p.IdReporte);

            builder.HasIndex(p => p.TipoBusqueda);
            builder.HasIndex(p => p.Estatus);
            builder.HasIndex(p => p.FechaInicio);
        }
    }
}