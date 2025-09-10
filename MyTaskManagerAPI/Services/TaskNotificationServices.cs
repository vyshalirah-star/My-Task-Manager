using Microsoft.EntityFrameworkCore;
using MyTaskManagerAPI.Data;

namespace MyTaskManagerAPI.Services
{
    public class TaskNotificationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TaskNotificationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyTaskManagerContext>();

                var upcomingTasks = await context.Tasks
                    .Where(t => !t.IsCompleted && t.DueDate <= DateTime.UtcNow.AddHours(24))
                    .ToListAsync();

                foreach (var task in upcomingTasks)
                {
                    // For testing: just log in console
                    Console.WriteLine($"Reminder: Task '{task.Title}' is due on {task.DueDate} .Kindly update them");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Run every hour
            }
        }
    }
}
