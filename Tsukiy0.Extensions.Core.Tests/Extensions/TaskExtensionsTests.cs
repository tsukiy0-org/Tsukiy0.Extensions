using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using System.Threading;

namespace Tsukiy0.Extensions.Core.Tests.Extensions
{
    public class TaskExtensionsTests
    {
        public class WhenAllBatched
        {
            [Fact]
            public async Task BatchesTasks()
            {
                var sw = new Stopwatch();
                var tasks = Enumerable.Range(0, 2).Select(async _ => Thread.Sleep(1000));

                sw.Start();
                await Core.Extensions.TaskExtensions.WhenAllBatched(tasks, 1);
                sw.Stop();

                sw.ElapsedMilliseconds.Should().BeCloseTo(2000, 100);
            }
        }
    }
}
