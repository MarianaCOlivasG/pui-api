using FluentValidation;

namespace PUI.Application.UseCases.Reportes.Commands.ActivarReporte
{
    public class ActivarReporteCommandValidator : AbstractValidator<ActivarReporteCommand>
    {
        public ActivarReporteCommandValidator()
        {
            RuleFor(p => p.Curp)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .Length(18).WithMessage("El campo {PropertyName} debe tener {MaxLength} caracteres");

            RuleFor(p => p.LugarNacimiento)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .MaximumLength(20).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

            RuleFor(p => p.Nombre)
                .MaximumLength(50).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

            RuleFor(p => p.PrimerApellido)
                .MaximumLength(50).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

            RuleFor(p => p.SegundoApellido)
                .MaximumLength(50).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

            RuleFor(p => p.Telefono)
                .MaximumLength(15).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

            RuleFor(p => p.Correo)
                .EmailAddress().When(p => !string.IsNullOrWhiteSpace(p.Correo))
                .MaximumLength(50).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

            RuleFor(p => p.FechaNacimiento)
                .Matches(@"^\d{4}-\d{2}-\d{2}$")
                .When(p => !string.IsNullOrWhiteSpace(p.FechaNacimiento))
                .WithMessage("El campo {PropertyName} debe tener formato YYYY-MM-DD");

            RuleFor(p => p.FechaDesaparicion)
                .Matches(@"^\d{4}-\d{2}-\d{2}$")
                .When(p => !string.IsNullOrWhiteSpace(p.FechaDesaparicion))
                .WithMessage("El campo {PropertyName} debe tener formato YYYY-MM-DD");

            RuleFor(p => p.SexoAsignado)
                .Must(s => s == "H" || s == "M" || s == "X")
                .When(p => !string.IsNullOrWhiteSpace(p.SexoAsignado))
                .WithMessage("El campo {PropertyName} debe ser H, M o X");

            RuleFor(p => p.CodigoPostal)
                .MaximumLength(5).WithMessage("El campo {PropertyName} no puede exceder {MaxLength} caracteres");

        }
    }
}