using PUI.Domain.Enums;
using PUI.Domain.Exceptions;
using PUI.Domain.ValueObjects;

namespace PUI.Domain.Entities
{
    public class NotificacionPam
    {
        public Guid Id { get; private set; }

        public Curp Curp { get; private set; } = null!;

        public string? Nombre { get; private set; }

        public string? PrimerApellido { get; private set; }

        public string? SegundoApellido { get; private set; }

        public string? TipoEvento { get; private set; }

        public DateTime? FechaEvento { get; private set; }

        public string? DescripcionLugarEvento { get; private set; }

        public string Origen { get; private set; } = "PAM";

        public EstatusProcesamientoNotificacion EstatusProcesamiento { get; private set; }

        public string? ErrorDetalle { get; private set; }

        public Guid? IdReporte { get; private set; }

        public DateTime FechaCreacion { get; private set; }

        public DateTime FechaActualizacion { get; private set; }

        public Reporte? Reporte { get; private set; }

        private NotificacionPam() { }

        public NotificacionPam(
            Curp curp,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            string? tipoEvento,
            DateTime? fechaEvento,
            string? descripcionLugarEvento
        )
        {
            Validar(curp);

            Id = Guid.NewGuid();
            Curp = curp;
            Nombre = nombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            TipoEvento = tipoEvento;
            FechaEvento = fechaEvento;
            DescripcionLugarEvento = descripcionLugarEvento;

            EstatusProcesamiento = EstatusProcesamientoNotificacion.PENDIENTE;

            FechaCreacion = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        private void Validar(Curp curp)
        {
            if (curp is null)
                throw new ExcepcionReglaNegocio("La CURP es requerida.");
        }

        public void MarcarComoProcesado(Guid? idReporte)
        {
            IdReporte = idReporte;
            EstatusProcesamiento = EstatusProcesamientoNotificacion.PROCESADO;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void MarcarComoError(string error)
        {
            ErrorDetalle = error;
            EstatusProcesamiento = EstatusProcesamientoNotificacion.ERROR;
            FechaActualizacion = DateTime.UtcNow;
        }
    }
}