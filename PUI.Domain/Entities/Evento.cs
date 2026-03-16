using PUI.Domain.Exceptions;
using PUI.Domain.ValueObjects;

namespace PUI.Domain.Entities
{
    public class Evento
    {
        public Guid Id { get; private set; }

        public string TipoEvento { get; private set; } = null!;

        public Guid? IdReporte { get; private set; }

        public Curp? Curp { get; private set; }

        public string? Resultado { get; private set; }

        public string? Descripcion { get; private set; }

        public string Origen { get; private set; } = null!;

        public DateTime FechaEvento { get; private set; }

        public Reporte? Reporte { get; private set; }

        private Evento() { }

        public Evento(
            string tipoEvento,
            string origen,
            Guid? idReporte = null,
            Curp? curp = null,
            string? resultado = null,
            string? descripcion = null
        )
        {
            ValidarReglas(tipoEvento, origen);

            Id = Guid.NewGuid();
            TipoEvento = tipoEvento;
            Origen = origen;
            IdReporte = idReporte;
            Curp = curp;
            Resultado = resultado;
            Descripcion = descripcion;

            FechaEvento = DateTime.UtcNow;
        }

        private void ValidarReglas(string tipoEvento, string origen)
        {
            if (string.IsNullOrWhiteSpace(tipoEvento))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(TipoEvento)}' es requerido.");

            if (string.IsNullOrWhiteSpace(origen))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(Origen)}' es requerido.");
        }
    }
}