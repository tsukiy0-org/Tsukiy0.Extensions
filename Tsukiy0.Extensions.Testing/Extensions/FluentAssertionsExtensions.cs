using System;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Tsukiy0.Extensions.Testing.Extensions
{
    public static class FluentAssertionsExtensions
    {
        public static EquivalencyAssertionOptions<T> ComparingDateTimesCloseTo<T>(this EquivalencyAssertionOptions<T> options, TimeSpan precision)
        {
            return options
                    .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision))
                    .WhenTypeIs<DateTimeOffset>()
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision))
                    .WhenTypeIs<DateTime>();
        }
    }
}
