using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;
using PUI.Persistence.Converters;

namespace PUI.Persistence.Configurations
{
    public class CoincidenciaConfig : IEntityTypeConfiguration<Coincidencia>
    {
        public void Configure(EntityTypeBuilder<Coincidencia> builder)
        {
            var encryptString = new EncryptStringConverter();
            var encryptDate = new EncryptDateConverter();

            builder.ToTable("pui_coincidencias");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id_coincidencia")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(c => c.FolioNotificacionPui)
                .HasColumnName("folio_notificacion_pui")
                .HasMaxLength(75)
                .IsRequired();

            builder.HasIndex(c => c.FolioNotificacionPui)
                .IsUnique();

            builder.Property(c => c.IdReporte)
                .HasColumnName("id_reporte")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(c => c.FaseBusqueda)
                .HasColumnName("fase_busqueda")
                .HasConversion<string>()
                .HasMaxLength(1)
                .IsRequired();

            builder.OwnsOne(c => c.CurpEncontrada, curp =>
            {
                curp.Property(x => x.Valor)
                    .HasColumnName("curp_encontrada")
                    .HasConversion(encryptString)
                    .HasMaxLength(255)
                    .IsRequired();
            });

            builder.Property(c => c.LugarNacimientoEncontrado)
                .HasColumnName("lugar_nacimiento_encontrado")
                .HasMaxLength(100);

            builder.Property(c => c.GuestId)
                .HasColumnName("guest_id")
                .HasMaxLength(100);

            builder.Property(c => c.TipoEvento)
                .HasColumnName("tipo_evento")
                .HasMaxLength(500);

            builder.Property(c => c.FechaEvento)
                .HasColumnName("fecha_evento")
                .HasConversion(encryptDate);

            builder.Property(c => c.DescripcionLugarEvento)
                .HasColumnName("descripcion_lugar_evento")
                .HasMaxLength(500);

            builder.Property(c => c.DetalleNotificacionPui)
                .HasColumnName("detalle_notificacion_pui")
                .HasColumnType("json");

            builder.Property(c => c.NivelCoincidencia)
                .HasColumnName("nivel_coincidencia")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.NotificadoPui)
                .HasColumnName("notificado_pui");

            builder.Property(c => c.FechaNotificacion)
                .HasColumnName("fecha_notificacion");

            builder.Property(c => c.EstatusNotificacion)
                .HasColumnName("estatus_notificacion")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.LogRespuestaPui)
                .HasColumnName("log_respuesta_pui");

            builder.Property(c => c.FechaDetectada)
                .HasColumnName("fecha_detectada");

            builder.Property(c => c.FechaActualizacion)
                .HasColumnName("fecha_actualizacion");

            builder.HasOne(c => c.Reporte)
                .WithMany()
                .HasForeignKey(c => c.IdReporte);
        }
    }
}