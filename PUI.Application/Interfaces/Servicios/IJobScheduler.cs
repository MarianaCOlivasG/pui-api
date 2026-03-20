
namespace PUI.Application.Interfaces.Servicios
{
    public interface IJobScheduler
    {
        Task EnqueueAsync(Func<Task> job);
        Task ScheduleAsync(Func<Task> job, TimeSpan delay);
    }
}
