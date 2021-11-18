using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using Tsukiy0.Extensions.Core.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Core.Tests.Extensions
{
    public class TaskExtensionsTests
    {
        public class WhenAllBatched
        {
            [Fact]
            public async Task WhenAllBatched__BehavesLikeWhenAll()
            {
                // Arrange
                var whenAllTasks = GetTasks();
                var whenAllBatchedTasks = GetTasks();

                // Act
                var whenAllResults = await Task.WhenAll(whenAllTasks);
                var whenAllBatchedResults = await whenAllBatchedTasks.WhenAllBatched(2);

                // Assert
                whenAllBatchedResults.Should().BeEquivalentTo(whenAllResults);
            }

            private static IEnumerable<Task<int>> GetTasks()
            {
                return new List<Task<int>>{
                    Task.FromResult(1),
                    Task.FromResult(2),
                    Task.FromResult(3),
                    Task.FromResult(4),
                    Task.FromResult(5),
                };
            }
        }
    }
}