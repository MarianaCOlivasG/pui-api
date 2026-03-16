
using PUI.Application.UseCases.Auth.Dtos;

namespace PUI.Application.Interfaces.Auth
{
    public interface IAuthService
    {

        Task<UsuarioAutenticadoDto> RegisterAsync(CredencialesUsuarioDto dto);
        Task<UsuarioAutenticadoDto> LoginAsync(CredencialesUsuarioDto dto);


    }
}
