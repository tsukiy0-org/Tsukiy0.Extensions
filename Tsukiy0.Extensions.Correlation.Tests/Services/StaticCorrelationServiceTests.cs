using System;

using FluentAssertions;

using Tsukiy0.Extensions.Correlation.Services;

using Xunit;

namespace Tsukiy0.Extensions.Correlation.Tests.Services
{
    public class StaticCorrelationServiceTests
    {
        [Fact]
        public void UsesProvidedTraceIdAndSpanId()
        {
            var traceId = Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var sut = new StaticCorrelationService(traceId, spanId);

            sut.TraceId.Should().Be(traceId);
            sut.SpanId.Should().Be(spanId);
        }
    }
}