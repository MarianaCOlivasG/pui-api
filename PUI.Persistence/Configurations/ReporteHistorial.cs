using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;

namespace PUI.Persistence.Configurations
{
    public class ReporteHistorialConfig : IEntityTypeConfiguration<ReporteHistorial>
    {
        public void Configure(EntityTypeBuilder<ReporteHistorial> builder)
        {
            builder.ToTable("pui_reportes_historial");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                .HasColumnName("id_historial")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(h => h.IdReporte)
                .HasColumnName("id_reporte")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(h => h.EstatusAnterior)
                .HasColumnName("estatus_anterior")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(h => h.EstatusNuevo)
                .HasColumnName("estatus_nuevo")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(h => h.Motivo)
                .HasColumnName("motivo");

            builder.Property(h => h.FechaCambio)
                .HasColumnName("fecha_cambio");

            builder.HasOne(h => h.Reporte)
                .WithMany()
                .HasForeignKey(h => h.IdReporte)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}