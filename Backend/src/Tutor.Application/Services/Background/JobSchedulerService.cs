// JobSchedulerService.cs

using Hangfire;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Tutor.Application.Services.Background;

public class JobSchedulerService : IHostedService
{
    private readonly IRecurringJobManager _recurringJobManager;

    public JobSchedulerService(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Schedule your recurring jobs here
        _recurringJobManager.AddOrUpdate<BookingNotificationService>(
            "booking-reminders",
            service => service.CheckUpcomingBookings(),
            "*/1 * * * *"); // Every 5 minutes

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}