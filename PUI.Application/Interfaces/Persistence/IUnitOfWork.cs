

namespace PUI.Application.Interfaces.Persistence
{

   public interface IUnitOfWork
    {
        Task Persistir();
        
        Task Reversar();
    }


}