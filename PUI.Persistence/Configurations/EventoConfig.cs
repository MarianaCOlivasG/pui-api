using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;
using PUI.Persistence.Converters;

namespace PUI.Persistence.Configurations
{
    public class EventoConfig : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            var encryptString = new EncryptStringConverter();

            builder.ToTable("pui_eventos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id_evento")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(e => e.TipoEvento)
                .HasColumnName("tipo_evento")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.IdReporte)
                .HasColumnName("id_reporte")
                .HasMaxLength(36);

            builder.Property(e => e.Resultado)
                .HasColumnName("resultado")
                .HasMaxLength(20);

            builder.Property(e => e.Descripcion)
                .HasColumnName("descripcion");

            builder.Property(e => e.Origen)
                .HasColumnName("origen")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.FechaEvento)
                .HasColumnName("fecha_evento");

            builder.OwnsOne(e => e.Curp, curp =>
            {
                curp.Property(c => c.Valor)
                    .HasColumnName("curp")
                    .HasConversion(encryptString)
                    .IsRequired(false);
            });

            builder.Navigation(e => e.Curp)
                   .IsRequired(false);

            builder.HasOne(e => e.Reporte)
                .WithMany()
                .HasForeignKey(e => e.IdReporte);
        }
    }
}