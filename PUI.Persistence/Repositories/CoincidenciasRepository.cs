using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{

    public class CoincidenciasRepository : Repository<Coincidencia>, ICoincidenciasRepository
    {
        public CoincidenciasRepository(PUIDbContext dbContext) : base(dbContext)
        {
        }
    }
}
