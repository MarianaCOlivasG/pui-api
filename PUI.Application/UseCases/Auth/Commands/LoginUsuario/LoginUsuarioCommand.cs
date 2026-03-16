using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Auth.Commands.LoginUsuario
{
    public class LoginUsuarioCommand : IRequest<UsuarioAutenticadoDto>
    {
        public required CredencialesUsuarioDto Credenciales { get; set; }
    }
}
