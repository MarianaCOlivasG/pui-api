using PUI.Application.UseCases.Auth.Commands.LoginUsuario;
using FluentValidation;

namespace PUI.Application.UseCases.Auth.LoginUsuario
{
    public class LoginUsuarioCommandValidator : AbstractValidator<LoginUsuarioCommand>
    {
        public LoginUsuarioCommandValidator()
        {
            RuleFor(x => x.Credenciales.UserName)
                .NotEmpty().WithMessage("El email es obligatorio.");

            RuleFor(x => x.Credenciales.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");
        }
    }
}
