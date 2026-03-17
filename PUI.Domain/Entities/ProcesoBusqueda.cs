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
            ValidarTipoBusqueda(tipoBusqueda);

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

        public void SumarEvaluados(int cantidad)
        {
            if (cantidad < 0)
                throw new ExcepcionReglaNegocio("Cantidad inválida para registros evaluados.");

            RegistrosEvaluados += cantidad;
        }

        public void SumarCoincidencias(int cantidad)
        {
            if (cantidad < 0)
                throw new ExcepcionReglaNegocio("Cantidad inválida para coincidencias.");

            CoincidenciasDetectadas += cantidad;
        }

        public void Completar()
        {
            ValidarTransicion();

            Estatus = EstatusProcesoBusqueda.COMPLETADO;
            FechaFin = DateTime.UtcNow;
        }

        public void MarcarError(string errorDetalle)
        {
            ValidarTransicion();

            if (string.IsNullOrWhiteSpace(errorDetalle))
                throw new ExcepcionReglaNegocio("El error no puede estar vacío.");

            if (errorDetalle.Length > 2000)
                errorDetalle = errorDetalle.Substring(0, 2000);

            Estatus = EstatusProcesoBusqueda.ERROR;
            ErrorDetalle = errorDetalle;
            FechaFin = DateTime.UtcNow;
        }

        public TimeSpan? ObtenerDuracion()
        {
            if (FechaFin == null)
                return null;

            return FechaFin.Value - FechaInicio;
        }


        public static ProcesoBusqueda CrearContinua(Guid? idReporte = null)
        {
            return new ProcesoBusqueda(TipoBusqueda.CONTINUA, idReporte);
        }

        public static ProcesoBusqueda CrearHistorica(Guid? idReporte = null)
        {
            return new ProcesoBusqueda(TipoBusqueda.HISTORICA, idReporte);
        }


        private void ValidarTipoBusqueda(TipoBusqueda tipoBusqueda)
        {
            if (!Enum.IsDefined(typeof(TipoBusqueda), tipoBusqueda))
                throw new ExcepcionReglaNegocio($"El tipo de búsqueda '{tipoBusqueda}' no es válido.");
        }

        private void ValidarTransicion()
        {
            if (Estatus != EstatusProcesoBusqueda.PROCESANDO)
                throw new ExcepcionReglaNegocio("El proceso ya fue finalizado.");
        }

    }
}