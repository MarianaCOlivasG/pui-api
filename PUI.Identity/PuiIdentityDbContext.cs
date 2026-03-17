using Microsoft.EntityFrameworkCore;
using PUI.Identity.Models;

namespace PUI.Identity
{
    public class PuiIdentityDbContext : DbContext
    {
        public PuiIdentityDbContext(DbContextOptions<PuiIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(entity =>
            {
                entity.ToTable("pui_usuarios");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                    .HasColumnName("id_usuario")
                    .HasMaxLength(36);

                entity.Property(u => u.UserName)
                    .HasColumnName("usuario")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.NormalizedUserName)
                    .HasColumnName("usuario_normalizado")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasColumnName("email")
                    .HasMaxLength(150);

                entity.Property(u => u.NormalizedEmail)
                    .HasColumnName("email_normalizado")
                    .HasMaxLength(150);

                entity.Property(u => u.PasswordHash)
                    .HasColumnName("pass")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.Property(u => u.IntentosFallidos)
                    .HasColumnName("intentos_fallidos")
                    .HasDefaultValue(0);

                entity.Property(u => u.BloqueoHasta)
                    .HasColumnName("bloqueo_hasta");

                entity.Property(u => u.UltimoLogin)
                    .HasColumnName("ultimo_login");

                entity.Property(u => u.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.FechaActualizacion)
                    .HasColumnName("fecha_actualizacion");

                // índices importantes
                entity.HasIndex(u => u.NormalizedUserName)
                    .IsUnique();

                entity.HasIndex(u => u.NormalizedEmail);
            });
        }
    }
}