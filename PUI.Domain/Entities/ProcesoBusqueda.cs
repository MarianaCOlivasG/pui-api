
using PUI.Domain.Enums;
using PUI.Domain.Exceptions;

namespace PUI.Domain.Entities
{
    public class ProcesoBusqueda
    {
        public Guid Id { get; private set; }

        public Guid? IdReporte { get; private set; }

        public TipoBusqueda TipoBusqueda { get; private set; }

        public int RegistrosEvaluados { get; private set; }

        public int CoincidenciasDetectadas { get; private set; }

        public DateTime FechaInicio { get; private set; }

        public DateTime? FechaFin { get; private set; }

        public EstatusProcesoBusqueda Estatus { get; private set; }

        public string? ErrorDetalle { get; private set; }

        public Reporte? Reporte { get; private set; }

        private ProcesoBusqueda() { }

        public ProcesoBusqueda(
            TipoBusqueda tipoBusqueda,
            Guid? idReporte = null
        )
        {
            ValidarReglas(tipoBusqueda);

            Id = Guid.NewGuid();
            TipoBusqueda = tipoBusqueda;
            IdReporte = idReporte;

            RegistrosEvaluados = 0;
            CoincidenciasDetectadas = 0;

            Estatus = EstatusProcesoBusqueda.PROCESANDO;
            FechaInicio = DateTime.UtcNow;
        }

        public void IncrementarEvaluados()
        {
            RegistrosEvaluados++;
        }

        public void IncrementarCoincidencias()
        {
            CoincidenciasDetectadas++;
        }

        public void Completar()
        {
            Estatus = EstatusProcesoBusqueda.COMPLETADO;
            FechaFin = DateTime.UtcNow;
        }

        public void MarcarError(string errorDetalle)
        {
            Estatus = EstatusProcesoBusqueda.ERROR;
            ErrorDetalle = errorDetalle;
            FechaFin = DateTime.UtcNow;
        }

        private void ValidarReglas(TipoBusqueda tipoBusqueda)
        {
            if (!Enum.IsDefined(typeof(TipoBusqueda), tipoBusqueda))
                throw new ExcepcionReglaNegocio($"El campo '{nameof(TipoBusqueda)}' no es válido.");
        }
    }
}