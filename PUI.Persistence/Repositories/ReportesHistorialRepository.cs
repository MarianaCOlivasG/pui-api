

using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{
    public class ReportesHistorialRepository : Repository<ReporteHistorial>, IReportesHistorialRepository
    {
        public ReportesHistorialRepository(PUIDbContext dbContext) : base(dbContext)
        {
        }
    }

}