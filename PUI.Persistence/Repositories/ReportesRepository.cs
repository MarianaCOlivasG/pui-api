

using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{


    public class ReportesRepository : Repository<Reporte>, IReportesRepository
    {
        public ReportesRepository(PUIDbContext dbContext) : base(dbContext)
        {
        }
    }

}