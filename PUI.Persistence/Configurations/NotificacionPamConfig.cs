using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;
using PUI.Persistence.Converters;

namespace PUI.Persistence.Configurations
{
    public class NotificacionPamConfig : IEntityTypeConfiguration<NotificacionPam>
    {
        public void Configure(EntityTypeBuilder<NotificacionPam> builder)
        {
            var encryptString = new EncryptStringConverter();

            builder.ToTable("pui_notificaciones_pam");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .HasColumnName("id_notificacion")
                .HasMaxLength(36)
                .IsRequired();

            // CURP
            builder.OwnsOne(r => r.Curp, curp =>
            {
                curp.Property(c => c.Valor)
                    .HasColumnName("curp")
                    .HasConversion(encryptString)
                    .IsRequired();
            });

            builder.Property(n => n.Nombre)
                .HasColumnName("nombre")
                .HasConversion(encryptString)
                .HasMaxLength(255);

            builder.Property(n => n.PrimerApellido)
                .HasColumnName("primer_apellido")
                .HasConversion(encryptString)
                .HasMaxLength(255);

            builder.Property(n => n.SegundoApellido)
                .HasColumnName("segundo_apellido")
                 .HasConversion(encryptString)
                .HasMaxLength(255);

            builder.Property(n => n.TipoEvento)
                .HasColumnName("tipo_evento")
                .HasMaxLength(500);

            builder.Property(n => n.FechaEvento)
                .HasColumnName("fecha_evento");

            builder.Property(n => n.DescripcionLugarEvento)
                .HasColumnName("descripcion_lugar_evento")
                .HasMaxLength(500);

            builder.Property(n => n.Origen)
                .HasColumnName("origen")
                .HasMaxLength(50)
                .HasDefaultValue("PAM");

            builder.Property(n => n.EstatusProcesamiento)
                .HasColumnName("estatus_procesamiento")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(n => n.ErrorDetalle)
                .HasColumnName("error_detalle");

            builder.Property(n => n.IdReporte)
                .HasColumnName("id_reporte")
                .HasMaxLength(36);

            builder.Property(n => n.FechaCreacion)
                .HasColumnName("fecha_creacion");

            builder.Property(n => n.FechaActualizacion)
                .HasColumnName("fecha_actualizacion");

            builder.HasOne(n => n.Reporte)
                .WithMany()
                .HasForeignKey(n => n.IdReporte)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}