using System;

namespace PUI.Persistence.Exceptions
{
    public class ExcepcionDePersistencia : Exception
    {
        public string? Detalle { get; }
        public string? CodigoError { get; }

        public ExcepcionDePersistencia(
            string mensaje,
            string? detalle = null,
            string? codigoError = null,
            Exception? innerException = null)
            : base(mensaje, innerException)
        {
            Detalle = detalle;
            CodigoError = codigoError;
        }

        public override string ToString()
        {
            return $"""
            Mensaje: {Message}
            CódigoError: {CodigoError}
            Detalle: {Detalle}
            InnerException: {InnerException?.Message}
            StackTrace: {StackTrace}
            """;
        }
    }
}