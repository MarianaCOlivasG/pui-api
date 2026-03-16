using PUI.Application.Interfaces.Auth;
using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Auth.Commands.LoginUsuario
{
    public class LoginUsuarioUseCase : IRequestHandler<LoginUsuarioCommand, UsuarioAutenticadoDto>
    {
        private readonly IAuthService authService;

        public LoginUsuarioUseCase(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<UsuarioAutenticadoDto> Handle(LoginUsuarioCommand request)
        {
            return await authService.LoginAsync(request.Credenciales);
        }
    }
}
