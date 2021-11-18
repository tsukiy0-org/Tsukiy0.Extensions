using System;

using FluentAssertions;
using FluentAssertions.Extensions;

using Tsukiy0.Extensions.Testing.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Testing.Tests.Extensions
{
    public class FluentAssertionExtensionsTests
    {
        [Fact]
        public void ComparingDateTimesCloseTo()
        {
            var input = new
            {
                DateTime = DateTime.UtcNow,
                DateTimeOffset = DateTimeOffset.UtcNow,
            };

            input.Should().BeEquivalentTo(new
            {
                DateTime = DateTime.UtcNow,
                DateTimeOffset = DateTimeOffset.UtcNow,
            }, o => o.ComparingDateTimesCloseTo(10.Seconds()));
        }

    }
}