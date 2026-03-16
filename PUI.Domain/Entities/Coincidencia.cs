using PUI.Domain.Enums;
using PUI.Domain.Exceptions;
using PUI.Domain.ValueObjects;

namespace PUI.Domain.Entities
{
    public class Coincidencia
    {
        public Guid Id { get; private set; }

        public string FolioNotificacionPui { get; private set; } = null!;

        public Guid IdReporte { get; private set; }

        public FaseBusqueda FaseBusqueda { get; private set; }

        public Curp CurpEncontrada { get; private set; } = null!;

        public string LugarNacimientoEncontrado { get; private set; } = null!;

        public string GuestId { get; private set; } = null!;

        public string? TipoEvento { get; private set; }

        public DateTime? FechaEvento { get; private set; }

        public string? DescripcionLugarEvento { get; private set; }

        public string? DetalleNotificacionPui { get; private set; }

        public NivelCoincidencia NivelCoincidencia { get; private set; }

        public bool NotificadoPui { get; private set; }

        public DateTime? FechaNotificacion { get; private set; }

        public EstatusNotificacion EstatusNotificacion { get; private set; }

        public string? LogRespuestaPui { get; private set; }

        public DateTime FechaDetectada { get; private set; }

        public DateTime FechaActualizacion { get; private set; }

        public Reporte Reporte { get; private set; } = null!;

        private Coincidencia() { }

        public Coincidencia(
            string folioNotificacionPui,
            Guid idReporte,
            FaseBusqueda faseBusqueda,
            Curp curpEncontrada,
            string lugarNacimientoEncontrado,
            string guestId,
            NivelCoincidencia nivelCoincidencia
        )
        {
            ValidarReglas(folioNotificacionPui, idReporte, curpEncontrada);

            Id = Guid.NewGuid();
            FolioNotificacionPui = folioNotificacionPui;
            IdReporte = idReporte;
            FaseBusqueda = faseBusqueda;
            CurpEncontrada = curpEncontrada;
            LugarNacimientoEncontrado = lugarNacimientoEncontrado;
            GuestId = guestId;
            NivelCoincidencia = nivelCoincidencia;

            EstatusNotificacion = EstatusNotificacion.PENDIENTE;
            NotificadoPui = false;

            FechaDetectada = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        private void ValidarReglas(
            string folioNotificacionPui,
            Guid idReporte,
            Curp curpEncontrada)
        {
            if (string.IsNullOrWhiteSpace(folioNotificacionPui))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(FolioNotificacionPui)}' es requerido.");

            if (idReporte == Guid.Empty)
                throw new ExcepcionReglaNegocio($"El campo '{nameof(IdReporte)}' es requerido.");

            if (curpEncontrada is null)
                throw new ExcepcionReglaNegocio($"El campo '{nameof(CurpEncontrada)}' es requerido.");
        }
    }
}