

using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{

    public class ApiLogsRepository : Repository<ApiLog>, IApiLogsRepository
    {
        public ApiLogsRepository(PUIDbContext dbContext) : base(dbContext)
        {
        }
    }

}