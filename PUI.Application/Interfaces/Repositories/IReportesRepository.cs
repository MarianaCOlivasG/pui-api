


using PUI.Domain.Entities;

namespace PUI.Application.Interfaces.Repositories
{

    public interface IReportesRepository: IRepository<Reporte>
    {

        Task<Reporte?> ObtenerPorFolioPUI(string folioPUI);
        Task<List<Reporte>> ObtenerParaBusquedaContinua(int minutos);


    }

}