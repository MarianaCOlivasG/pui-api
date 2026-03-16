using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;
using PUI.Persistence.Converters;

namespace PUI.Persistence.Configurations
{
    public class ReporteConfig : IEntityTypeConfiguration<Reporte>
    {
        public void Configure(EntityTypeBuilder<Reporte> builder)
        {

            var encryptString = new EncryptStringConverter();
            var encryptDate = new EncryptDateConverter();

            builder.ToTable("pui_reportes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id_reporte")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(r => r.FolioPui)
                .HasColumnName("folio_pui")
                .HasMaxLength(75)
                .IsRequired();

            builder.Property(r => r.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(255)
                .HasConversion(encryptString);

            builder.Property(r => r.PrimerApellido)
                .HasColumnName("primer_apellido")
                .HasMaxLength(255)
                .HasConversion(encryptString);

            builder.Property(r => r.SegundoApellido)
                .HasColumnName("segundo_apellido")
                .HasMaxLength(255)
                .HasConversion(encryptString);

            builder.Property(r => r.LugarNacimiento)
               .HasColumnName("lugar_nacimiento");

            builder.Property(r => r.FechaNacimiento)
                .HasColumnName("fecha_nacimiento")
                .HasConversion(encryptDate);

            builder.Property(r => r.FechaDesaparicion)
                .HasColumnName("fecha_desaparicion")
                .HasConversion(encryptDate);

            builder.Property(r => r.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            builder.Property(r => r.FechaActivacion)
                .HasColumnName("fecha_activacion");

            builder.Property(r => r.FechaDesactivacion)
                .HasColumnName("fecha_desactivacion");

            builder.Property(r => r.Sexo)
                .HasColumnName("sexo")
                .HasConversion<string>()
                .HasMaxLength(1);

            builder.Property(r => r.Estatus)
                .HasColumnName("estatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.OwnsOne(r => r.Curp, curp =>
            {
                curp.Property(c => c.Valor)
                    .HasColumnName("curp")
                    .HasConversion(encryptString)
                    .IsRequired();
            });

            builder.OwnsOne(r => r.Correo, correo =>
            {
                correo.Property(c => c.Valor)
                    .HasColumnName("correo")
                    .HasConversion(encryptString)
                    .IsRequired(false);
            });

            builder.Navigation(r => r.Correo)
                   .IsRequired(false);


        }
    }
}

