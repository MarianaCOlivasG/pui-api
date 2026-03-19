using Microsoft.EntityFrameworkCore;
using PUI.Application.Interfaces.Repositories;
using PUI.Domain.Entities;
using PUI.Persistencia;


namespace PUI.Persistence.Repositories
{
    public class NotificacionesPamRepository : Repository<NotificacionPam>, INotificacionesPamRepository
    {
        protected readonly PUIDbContext _dbContext;
        public NotificacionesPamRepository(PUIDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<NotificacionPam>> ObtenerListadoPorCurp(string Curp)
        {
            return await _dbContext.NotificacionesPam
                .Where(r => r.Curp.Valor == Curp).ToListAsync();
        }

    } 

}
