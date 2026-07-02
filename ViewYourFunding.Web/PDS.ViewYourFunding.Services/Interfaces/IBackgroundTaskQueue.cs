using System;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Interface for a queue of background tasks.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Add a new task to the queue.
        /// </summary>
        /// <param name="task">The task to be queued.</param>
        void QueueTask(Func<CancellationToken, Task> task);

        /// <summary>
        /// Dequeue and return the next task.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The next task.</returns>
        Task<Func<CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken);
    }
}