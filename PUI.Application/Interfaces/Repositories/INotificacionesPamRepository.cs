using PUI.Domain.Entities;

namespace PUI.Application.Interfaces.Repositories
{
    public interface INotificacionesPamRepository : IRepository<NotificacionPam>
    {

        Task<List<NotificacionPam>> ObtenerListadoPorCurp(string Curp);


    }
}
