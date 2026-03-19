using FluentValidation;
using PUI.Application.UseCases.Reportes.Commands.ActivarReporte;

namespace PUI.Application.UseCases.NotificacionesPam.Commands.AgregarNotificacionPam
{
    public class AgregarNotificacionPamCommandValidator : AbstractValidator<ActivarReporteCommand>
    {

        public AgregarNotificacionPamCommandValidator()
        {
            RuleFor(p => p.Curp)
               .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
               .Length(18).WithMessage("El campo {PropertyName} debe tener {MaxLength} caracteres");
        }
    }
}
