using PUI.Domain.Enums;
using PUI.Domain.Exceptions;

namespace PUI.Domain.Entities
{
    public class ApiLog
    {
        public Guid Id { get; private set; }

        public string Endpoint { get; private set; } = null!;

        public string Metodo { get; private set; } = null!;

        public DireccionApi Direccion { get; private set; }

        public string? RequestBody { get; private set; }

        public string? ResponseBody { get; private set; }

        public int? HttpStatus { get; private set; }

        public string? IpOrigen { get; private set; }

        public DateTime FechaRequest { get; private set; }

        public int? DuracionMs { get; private set; }

        private ApiLog() { }

        public ApiLog(
            string endpoint,
            string metodo,
            DireccionApi direccion,
            string? requestBody = null,
            string? responseBody = null,
            int? httpStatus = null,
            string? ipOrigen = null,
            int? duracionMs = null
        )
        {
            ValidarReglas(endpoint, metodo);

            Id = Guid.NewGuid();
            Endpoint = endpoint;
            Metodo = metodo;
            Direccion = direccion;
            RequestBody = requestBody;
            ResponseBody = responseBody;
            HttpStatus = httpStatus;
            IpOrigen = ipOrigen;
            DuracionMs = duracionMs;

            FechaRequest = DateTime.UtcNow;
        }

        private void ValidarReglas(string endpoint, string metodo)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(Endpoint)}' es requerido.");

            if (string.IsNullOrWhiteSpace(metodo))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(Metodo)}' es requerido.");
        }
    }
}