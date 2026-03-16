using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PUI.Application.Interfaces.Persistence;
using PUI.Persistence.Exceptions;
using PUI.Persistencia;

namespace PUI.Persistence.UnitOfWorks
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly PUIDbContext dbContext;

        public EFCoreUnitOfWork(PUIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Persistir()
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mysql)
            {
                throw new ExcepcionDePersistencia(
                    "Error al guardar en la base de datos.",
                    mysql.Message,
                    mysql.Number.ToString(),
                    ex
                );            
            }
        }

        public Task Reversar()
        {
            return Task.CompletedTask;
        }
    }
}