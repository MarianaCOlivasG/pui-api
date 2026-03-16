using PUI.Application.Interfaces.Auth;
using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Auth.Commands.RegistrarUsuario
{
    public class RegistrarUsuarioUseCase : IRequestHandler<RegistrarUsuarioCommand, UsuarioAutenticadoDto>
    {
        private readonly IAuthService authService;

        public RegistrarUsuarioUseCase(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<UsuarioAutenticadoDto> Handle(RegistrarUsuarioCommand request)
        {
            return await authService.RegisterAsync(request.Credenciales);
        }
    }
}
