namespace PUI.Application.Utils.Paginacion
{
    public class ResultadoPaginadoDto<T>
    {
        public int TotalRegistros { get; init; }
        public int NumeroPagina { get; init; }
        public int RegistrosPorPagina { get; init; }
        public IReadOnlyList<T> Registros { get; init; } = new List<T>();

        public int TotalPaginas =>
            RegistrosPorPagina == 0
                ? 0
                : (int)Math.Ceiling((double)TotalRegistros / RegistrosPorPagina);

        public bool TienePaginaAnterior => NumeroPagina > 1;

        public bool TienePaginaSiguiente => NumeroPagina < TotalPaginas;
    }
}