namespace PUI.Infrastructure.Identity.Exceptions
{
    public class IdentityValidationException : Exception
    {
        public string[] Errores { get; }

        public IdentityValidationException(params string[] errores)
            : base("Se produjeron errores de validación en Identity")
        {
            Errores = errores;
        }
    }
}