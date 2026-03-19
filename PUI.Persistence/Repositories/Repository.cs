using Microsoft.EntityFrameworkCore;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Utils.Paginacion;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PUIDbContext dbContext;

        public Repository(PUIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> Query()
        {
            return dbContext.Set<T>().AsNoTracking();
        }

        public async Task Actualizar(T entidad)
        {
            dbContext.Update(entidad);
            await Task.CompletedTask;
        }

        public async Task<T> Agregar(T entidad)
        {
            await dbContext.AddAsync(entidad);
            return entidad;
        }

        public async Task Borrar(T entidad)
        {
            dbContext.Remove(entidad);
            await Task.CompletedTask;
        }

        public async Task<T?> ObtenerPorGuid(Guid id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ObtenerTodos()
        {
            return await dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ResultadoPaginadoDto<T>> ObtenerTodosPaginados(PaginacionDto paginacion)
        {
            var query = dbContext.Set<T>().AsNoTracking();

            return await ObtenerPaginadoInterno(query, paginacion);
        }

        public async Task<ResultadoPaginadoDto<T>> ObtenerPaginado(
            IQueryable<T> query,
            PaginacionDto paginacion)
        {
            return await ObtenerPaginadoInterno(query, paginacion);
        }

        protected async Task<ResultadoPaginadoDto<T>> ObtenerPaginadoInterno(
            IQueryable<T> query,
            PaginacionDto paginacion)
        {
            var totalRegistros = await query.CountAsync();

            var registros = await query
                .Skip(paginacion.Omitir)
                .Take(paginacion.RegistrosPorPagina)
                .ToListAsync();

            return new ResultadoPaginadoDto<T>
            {
                TotalRegistros = totalRegistros,
                NumeroPagina = paginacion.NumeroPagina,
                RegistrosPorPagina = paginacion.RegistrosPorPagina,
                Registros = registros
            };
        }
    }
}