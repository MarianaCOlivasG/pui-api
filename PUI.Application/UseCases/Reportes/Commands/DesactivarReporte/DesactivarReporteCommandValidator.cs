using FluentValidation;

namespace PUI.Application.UseCases.Reportes.Commands.DesactivarReporte
{
    public class DesactivarReporteCommandValidator : AbstractValidator<DesactivarReporteCommand>
    {
        public DesactivarReporteCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
        }
    }
}