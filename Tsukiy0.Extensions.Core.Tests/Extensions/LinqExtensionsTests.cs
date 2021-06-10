using System.Linq;
using FluentAssertions;
using Tsukiy0.Extensions.Core.Extensions;
using Xunit;

namespace Tsukiy0.Extensions.Core.Tests.Extensions
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void Chunk()
        {
            var input = Enumerable.Range(0, 100);

            var actual = input.Chunk(10);

            actual.Count().Should().Be(10);
            actual.First().Should().ContainInOrder(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            actual.Last().Should().ContainInOrder(90, 91, 92, 93, 94, 95, 96, 97, 98, 99);
        }

    }
}
