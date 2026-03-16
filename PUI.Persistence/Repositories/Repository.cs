

using PUI.Application.Interfaces.Repositories;
using PUI.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace PUI.Persistence.Repositories
{


    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PUIDbContext dbContext;

        public Repository(PUIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        Task IRepository<T>.Actualizar(T entidad)
        {
            dbContext.Update(entidad);
            return Task.CompletedTask;
        }

        Task<T> IRepository<T>.Agregar(T entidad)
        {
             dbContext.Add(entidad);
            return Task.FromResult(entidad);
        }

        Task IRepository<T>.Borrar(T entidad)
        {
             dbContext.Remove(entidad);
            return Task.CompletedTask;
        }

        async Task<T?> IRepository<T>.ObtenerPorGuid(Guid Id)
        {
            return await dbContext.Set<T>().FindAsync(Id);
        }

        async Task<IEnumerable<T>> IRepository<T>.ObtenerTodos()
        {
            return await dbContext.Set<T>().ToListAsync();
        }


    }

}