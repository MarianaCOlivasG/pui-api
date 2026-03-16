using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Auth.Commands.RegistrarUsuario
{
    public class RegistrarUsuarioCommand : IRequest<UsuarioAutenticadoDto>
    {
        public required CredencialesUsuarioDto Credenciales { get; set; }
    }
}
