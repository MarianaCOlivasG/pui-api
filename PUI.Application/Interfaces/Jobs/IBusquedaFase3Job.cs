

namespace PUI.Application.Interfaces.Jobs
{
    public interface IBusquedaFase3Job
    {
        Task Ejecutar(Guid reporteId);
    }
}
