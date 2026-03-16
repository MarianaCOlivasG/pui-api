
using FluentValidation.Results;

namespace PUI.Domain.Exceptions
{
    
    public class ExcepcionDeValidacion: Exception
    {
        
      public List<string> ErroresDeValidacion { get; set; } = [];


      public ExcepcionDeValidacion( string mensaje )
      {
        ErroresDeValidacion.Add(mensaje);
      }


      public ExcepcionDeValidacion( ValidationResult validationResult )
      {
        foreach (var error in validationResult.Errors )
        {
          ErroresDeValidacion.Add(error.ErrorMessage);
        }
      }

    }

}