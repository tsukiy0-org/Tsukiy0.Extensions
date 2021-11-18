using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<IEnumerable<T>> WhenAllBatched<T>(this IEnumerable<Task<T>> tasks, int maxConcurrency)
        {
            IEnumerable<T> results = new List<T>();
            foreach (var batch in tasks.Chunk(maxConcurrency))
            {
                var partialResults = await Task.WhenAll(batch);
                results = results.Concat(partialResults);
            }
            return results;

        }

        public static async Task WhenAllBatched(this IEnumerable<Task> tasks, int maxConcurrency)
        {
            await tasks
            .Select(async _ =>
            {
                await _;
                return true;
            })
            .WhenAllBatched(maxConcurrency);
        }
    }
}