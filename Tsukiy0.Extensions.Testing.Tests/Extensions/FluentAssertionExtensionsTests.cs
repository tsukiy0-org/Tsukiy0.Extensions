using System;
using Xunit;
using FluentAssertions;
using Tsukiy0.Extensions.Testing.Extensions;
using FluentAssertions.Extensions;

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
