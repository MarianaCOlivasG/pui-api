

using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{

    public class EventosRepository : Repository<Evento>, IEventosRepository
    {
        public EventosRepository(PUIDbContext dbContext) : base(dbContext)
        {
        }
    }

}