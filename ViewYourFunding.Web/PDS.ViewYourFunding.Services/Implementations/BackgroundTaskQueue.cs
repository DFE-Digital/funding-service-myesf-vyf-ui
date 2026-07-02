using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Implementation of a background task queue.
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _tasks =
            new ConcurrentQueue<Func<CancellationToken, Task>>();

        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        /// <inheritdoc/>
        public void QueueTask(Func<CancellationToken, Task> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _tasks.Enqueue(task);
            _signal.Release();
        }

        /// <inheritdoc/>
        public async Task<Func<CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _tasks.TryDequeue(out var task);

            return task;
        }
    }
}