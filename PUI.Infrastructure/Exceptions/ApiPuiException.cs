namespace PUI.Infrastructure.Infrastructure.Exceptions
{
    public class ApiPuiException : Exception
    {
        public int StatusCode { get; }
        public string? Codigo { get; }
        public object? Errores { get; }

        public ApiPuiException(
            string mensaje,
            int statusCode,
            string? codigo = null,
            object? errores = null
        ) : base(mensaje)
        {
            StatusCode = statusCode;
            Codigo = codigo;
            Errores = errores;
        }
    }
}