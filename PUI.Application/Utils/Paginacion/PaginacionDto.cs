namespace PUI.Application.Utils.Paginacion
{
    public record PaginacionDto
    {
        private const int MaximoRegistrosPorPagina = 100;
        private const int RegistrosPorPaginaPorDefecto = 10;

        public int NumeroPagina { get; }
        public int RegistrosPorPagina { get; }

        public int Omitir => (NumeroPagina - 1) * RegistrosPorPagina;

        public PaginacionDto(int numeroPagina = 1, int registrosPorPagina = RegistrosPorPaginaPorDefecto)
        {
            NumeroPagina = numeroPagina < 1 ? 1 : numeroPagina;

            RegistrosPorPagina = registrosPorPagina < 1
                ? RegistrosPorPaginaPorDefecto
                : Math.Min(registrosPorPagina, MaximoRegistrosPorPagina);
        }
    }
}