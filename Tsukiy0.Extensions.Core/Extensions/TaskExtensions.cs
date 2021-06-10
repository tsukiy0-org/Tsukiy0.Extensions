using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Tsukiy0.Extensions.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<IEnumerable<T>> WhenAllBatched<T>(IEnumerable<Task<T>> tasks, int maxConcurrency)
        {
            using var semaphore = new SemaphoreSlim(maxConcurrency);

            var pendingTasks = tasks.Select(async task =>
            {
                try
                {
                    semaphore.Wait();
                    return await task;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(pendingTasks);

            return pendingTasks.Select(_ => _.Result);
        }

        public static async Task WhenAllBatched(IEnumerable<Task> tasks, int maxConcurrency)
        {
            using var semaphore = new SemaphoreSlim(maxConcurrency);

            var pendingTasks = tasks.Select(async task =>
            {
                try
                {
                    semaphore.Wait();
                    await task;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(pendingTasks);
        }
    }
}
