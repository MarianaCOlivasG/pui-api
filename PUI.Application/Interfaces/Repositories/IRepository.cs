using PUI.Application.Utils.Paginacion;

namespace PUI.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> ObtenerPorGuid(Guid id);

        Task<IEnumerable<T>> ObtenerTodos();

        Task<T> Agregar(T entidad);

        Task Actualizar(T entidad);

        Task Borrar(T entidad);

        IQueryable<T> Query();

        Task<ResultadoPaginadoDto<T>> ObtenerPaginado(
            IQueryable<T> query,
            PaginacionDto paginacion
        );

        Task<ResultadoPaginadoDto<T>> ObtenerTodosPaginados(
            PaginacionDto paginacion
        );
    }
}