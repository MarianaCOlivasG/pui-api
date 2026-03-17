


using PUI.Domain.Entities;

namespace PUI.Application.Interfaces.Repositories
{

    public interface IReportesRepository: IRepository<Reporte>
    {

        Task<List<Reporte>> ObtenerParaBusquedaContinua(int minutos);


    }

}