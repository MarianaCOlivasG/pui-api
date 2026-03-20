
namespace PUI.Application.UseCases.Reportes.Orquestacion
{
    public interface IOrquestadorReportes
    {
        Task IniciarFlujoAsync(Guid reporteId);
    }
}
