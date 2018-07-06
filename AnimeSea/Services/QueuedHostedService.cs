using System;
using System.Threading;
using System.Threading.Tasks;
using AnimeSea.Services.BackgroundTasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnimeSea.Services
{
    public class QueuedHostedService : IHostedService
    {
        private readonly ILogger _logger;

        private readonly IBackgroundTaskQueue _taskQueue;

        private readonly CancellationTokenSource _shutdown;

        private Task _backgroundTask;

        public QueuedHostedService(
            ILogger<QueuedHostedService> logger,
            IBackgroundTaskQueue taskQueue)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            _shutdown = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");
            _backgroundTask = Task.Run(DoBackgroundProcessing, cancellationToken);

            return Task.CompletedTask;
        }

        private async Task DoBackgroundProcessing()
        {
            while (!_shutdown.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(_shutdown.Token);

                try
                {
                    await workItem(_shutdown.Token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while executing {nameof(workItem)}.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");
            _shutdown.Cancel();

            return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
