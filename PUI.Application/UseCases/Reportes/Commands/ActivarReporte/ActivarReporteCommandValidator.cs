


using FluentValidation;

namespace PUI.Application.UseCases.Reportes.Commands.ActivarReporte
{

    public class ActivarReporteCommandValidator : AbstractValidator<ActivarReporteCommand>
    {

        public ActivarReporteCommandValidator()
        {

            RuleFor(p => p.Curp)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .MaximumLength(150).WithMessage("La longitud del campo {PropertyName} deber ser menor o igual que {MaxLength}");

        }

    }

}