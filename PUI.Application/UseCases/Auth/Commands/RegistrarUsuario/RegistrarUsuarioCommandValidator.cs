

using FluentValidation;

namespace PUI.Application.UseCases.Auth.Commands.RegistrarUsuario
{
    public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
    {
        public RegistrarUsuarioCommandValidator()
        {
            RuleFor(x => x.Credenciales.UserName)
                .MinimumLength(3).WithMessage("El username debe tener al menos 3 caracteres")
                .NotEmpty().WithMessage("El username no es válido.");

            RuleFor(x => x.Credenciales.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.");
        }
    }
}
