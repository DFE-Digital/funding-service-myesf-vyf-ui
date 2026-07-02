using Microsoft.Extensions.Hosting;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The queued hosted service that runs work items retrieved from a task queue.
    /// </summary>
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILoggerAdapter<QueuedHostedService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueuedHostedService"/> class.
        /// </summary>
        /// <param name="taskQueue">The queue of background tasks to run.</param>
        /// <param name="logger">The logger.</param>
        public QueuedHostedService(
            IBackgroundTaskQueue taskQueue,
            ILoggerAdapter<QueuedHostedService> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _taskQueue.DequeueTask(stoppingToken);

                try
                {
                    await task(stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger?.LogError(
                        exception,
                        "Error occurred executing {task}.",
                        nameof(task));
                }
            }
        }
    }
}