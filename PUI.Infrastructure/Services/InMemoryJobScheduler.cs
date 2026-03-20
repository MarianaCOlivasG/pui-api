using PUI.Application.Interfaces.Servicios;

namespace PUI.Infrastructure.Services
{
    public class InMemoryJobScheduler : IJobScheduler
    {
        public Task EnqueueAsync(Func<Task> job)
        {
            _ = Task.Run(job);
            return Task.CompletedTask;
        }

        public async Task ScheduleAsync(Func<Task> job, TimeSpan delay)
        {
            await Task.Delay(delay);
            await job();
        }
    }
}
