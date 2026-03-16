using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PUI.Domain.Entities;

namespace PUI.Persistence.Configurations
{
    public class ApiLogConfig : IEntityTypeConfiguration<ApiLog>
    {
        public void Configure(EntityTypeBuilder<ApiLog> builder)
        {
            builder.ToTable("pui_api_logs");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .HasColumnName("id_log")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(l => l.Endpoint)
                .HasColumnName("endpoint")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(l => l.Metodo)
                .HasColumnName("metodo")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(l => l.Direccion)
                .HasColumnName("direccion")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(l => l.RequestBody)
                .HasColumnName("request_body")
                .HasColumnType("json");

            builder.Property(l => l.ResponseBody)
                .HasColumnName("response_body")
                .HasColumnType("json");

            builder.Property(l => l.HttpStatus)
                .HasColumnName("http_status");

            builder.Property(l => l.IpOrigen)
                .HasColumnName("ip_origen")
                .HasMaxLength(50);

            builder.Property(l => l.FechaRequest)
                .HasColumnName("fecha_request");

            builder.Property(l => l.DuracionMs)
                .HasColumnName("duracion_ms");

            builder.HasIndex(l => l.Endpoint);
            builder.HasIndex(l => l.HttpStatus);
            builder.HasIndex(l => l.FechaRequest);
        }
    }
}