using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Tsukiy0.Extensions.Logging.AspNetCore.Services;
using Xunit;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Tests.Services
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
        public void WhenNoHeader__ThenReturnNewTraceId()
        {
            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void WhenHasHeader__ThenReturnHeaderAsTraceId()
        {
            var traceId = Guid.NewGuid().ToString();
            context.Request.Headers[RequestCorrelationService.HEADER] = traceId;

            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().Be(traceId);
        }

        [Fact]
        public void WhenBadHeader__ThenReturnNewTraceId()
        {
            context.Request.Headers[RequestCorrelationService.HEADER] = "gibberish";

            var actual = new RequestCorrelationService(mockHttpContextAccessor.Object);

            actual.TraceId.Should().NotBe(Guid.Empty);
        }
    }
}
