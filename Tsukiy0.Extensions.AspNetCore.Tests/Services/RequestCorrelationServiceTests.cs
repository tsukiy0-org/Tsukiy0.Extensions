using System;

using FluentAssertions;

using Microsoft.AspNetCore.Http;

using Moq;

using Tsukiy0.Extensions.AspNetCore.Services;
using Tsukiy0.Extensions.Http.Constants;

using Xunit;

namespace Tsukiy0.Extensions.AspNetCore.Tests.Services
{
    public class RequestCorrelationServiceTests
    {
        private readonly HttpContext context;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;

        public RequestCorrelationServiceTests()
        {
            context = new DefaultHttpContext();
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.SetupGet(_ => _.HttpContext).Returns(context);
        }

        [Fact]
        public void SpanIdIsRandom()
        {
            var actual1 = new RequestCorrelationService(mockHttpContextAccessor.Object);
            var actual2 = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual1.SpanId.Should().NotBe(actual2.SpanId);
        }

        [Fact]
        public void WhenNoHeaderThenReturnNewTraceId()
        {
            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void WhenHasHeaderThenReturnHeaderAsTraceId()
        {
            var traceId = Guid.NewGuid().ToString();
            context.Request.Headers[HttpHeaders.TraceId] = traceId;

            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().Be(traceId);
        }

        [Fact]
        public void WhenBadHeaderThenReturnNewTraceId()
        {
            context.Request.Headers[HttpHeaders.TraceId] = "gibberish";

            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void NewTraceIdIsTheSameEverytime()
        {
            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().Be(actual.TraceId);
        }

        [Fact]
        public void NewSpanIdIsTheSameEverytime()
        {
            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.SpanId.Should().Be(actual.SpanId);
        }
    }
}