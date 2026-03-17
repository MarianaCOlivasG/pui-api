

using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{

    public class ProcesoBusquedaRepository : Repository<ProcesoBusqueda>, IProcesoBusquedaRepository
    {
        public ProcesoBusquedaRepository(PUIDbContext dbContext) : base(dbContext)
        {
        }
    }

}