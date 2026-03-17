namespace PUI.Identity.Models
{
    public class Usuario
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserName { get; set; } = null!;

        public string NormalizedUserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string NormalizedEmail { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool Activo { get; set; } = true;

        public int IntentosFallidos { get; set; } = 0;

        public DateTime? BloqueoHasta { get; set; }

        public DateTime? UltimoLogin { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaActualizacion { get; set; }
    }
}