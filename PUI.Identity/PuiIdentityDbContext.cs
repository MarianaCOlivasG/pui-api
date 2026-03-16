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

        // DbSet de usuarios
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(entity =>
            {
                // Nombre de la tabla
                entity.ToTable("pui_usuarios");

                // Clave primaria
                entity.HasKey(u => u.Id);

                // Mapear propiedades a columnas
                entity.Property(u => u.Id)
                    .HasColumnName("id_usuario")
                    .HasMaxLength(36)
                    .IsRequired();

                entity.Property(u => u.UserName)
                    .HasColumnName("usuario")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .HasColumnName("pass")
                    .HasMaxLength(255)
                    .IsRequired();
            });
        }
    }
}