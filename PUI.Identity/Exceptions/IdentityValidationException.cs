namespace PUI.Infrastructure.Identity.Exceptions
{
    public class ExcepcionValidacionIdentity : Exception
    {
        public string[] Errores { get; }

        public ExcepcionValidacionIdentity(params string[] errores)
            : base("Se produjeron errores de validación en Identity")
        {
            Errores = errores;
        }
    }
}