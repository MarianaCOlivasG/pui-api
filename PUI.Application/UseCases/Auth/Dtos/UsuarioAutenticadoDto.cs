namespace PUI.Application.UseCases.Auth.Dtos
{
    public class UsuarioAutenticadoDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }
}
