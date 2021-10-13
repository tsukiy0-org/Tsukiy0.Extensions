using FluentAssertions;
using System;
using Tsukiy0.Extensions.Correlation.Services;
using Xunit;

namespace Tsukiy0.Extensions.Correlation.Tests.Services
{
    public class StaticCorrelationServiceTests
    {
        [Fact]
        public void UsesProvidedCorrelationIdAndSpanId()
        {
            var correlationId = Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var sut = new StaticCorrelationService(correlationId, spanId);

            sut.TraceId.Should().Be(correlationId);
            sut.SpanId.Should().Be(spanId);
        }
    }
}
