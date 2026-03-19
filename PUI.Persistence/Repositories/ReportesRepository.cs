using Microsoft.EntityFrameworkCore;
using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Domain.Enums;
using PUI.Persistencia;

namespace PUI.Persistence.Repositories
{
    public class ReportesRepository : Repository<Reporte>, IReportesRepository
    {

        protected readonly PUIDbContext _dbContext;

        public ReportesRepository(PUIDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Reporte>> ObtenerParaBusquedaContinua(int minutos)
        {
            var limite = DateTime.UtcNow.AddMinutes(-minutos);

            return await _dbContext.Reportes
                .Where(r =>
                    r.Estatus == EstatusReporte.Activo &&
                    r.FechaDesactivacion == null &&
                    (r.FechaUltimaBusqueda == null || r.FechaUltimaBusqueda < limite)
                )
                .OrderBy(r => r.FechaUltimaBusqueda)
                .Take(100)
                .ToListAsync();
        }

        public async Task<Reporte?> ObtenerPorFolioPUI(string folioPUI)
        {
            return await _dbContext.Reportes.FirstOrDefaultAsync(r => r.FolioPui == folioPUI);
        }
    }
}