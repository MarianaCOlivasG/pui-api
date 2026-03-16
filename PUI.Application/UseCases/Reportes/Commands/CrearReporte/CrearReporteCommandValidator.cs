


using FluentValidation;

namespace PUI.Application.UseCases.Reportes.Commands.CrearReporte
{
    
    public class CrearReporteCommandValidator: AbstractValidator<CrearReporteCommand>
    {

        public CrearReporteCommandValidator()
        {
            
            RuleFor( p => p.Nombre )
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")                
                .MaximumLength(150).WithMessage("La longitud del campo {PropertyName} deber ser menor o igual que {MaxLength}");

        }

    }

}