using PUI.Domain.Enums;
using PUI.Domain.Exceptions;

namespace PUI.Domain.Entities
{
    public class ReporteHistorial
    {
        public Guid Id { get; private set; }

        public Guid IdReporte { get; private set; }

        public EstatusReporte? EstatusAnterior { get; private set; }

        public EstatusReporte EstatusNuevo { get; private set; }

        public string? Motivo { get; private set; }

        public DateTime FechaCambio { get; private set; }

        public Reporte Reporte { get; private set; } = null!;

        private ReporteHistorial() { }

        public ReporteHistorial(
            Guid idReporte,
            EstatusReporte estatusNuevo,
            EstatusReporte? estatusAnterior = null,
            string? motivo = null)
        {
            ValidarReglas(idReporte, estatusNuevo);

            Id = Guid.NewGuid();
            IdReporte = idReporte;
            EstatusAnterior = estatusAnterior;
            EstatusNuevo = estatusNuevo;
            Motivo = motivo;
            FechaCambio = DateTime.UtcNow;
        }

        private void ValidarReglas(Guid idReporte, EstatusReporte estatusNuevo)
        {
            if (idReporte == Guid.Empty)
                throw new ExcepcionReglaNegocio($"El campo '{nameof(IdReporte)}' es requerido.");

            if (!Enum.IsDefined(typeof(EstatusReporte), estatusNuevo))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(EstatusNuevo)}' no es válido.");
        }
    }
}