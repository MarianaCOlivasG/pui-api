


namespace PUI.Application.Interfaces.Repositories
{

    public interface IRepository<T> where T : class
    {
        
        Task<T?> ObtenerPorGuid(Guid Id);

        Task<IEnumerable<T>> ObtenerTodos();

        Task<T> Agregar( T entidad );

        Task Actualizar(T entidad);

        Task Borrar(T entidad);


    }

}