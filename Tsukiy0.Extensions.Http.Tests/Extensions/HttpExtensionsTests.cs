using System;
using System.Net;
using System.Net.Http;

using FluentAssertions;

using Moq;

using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Http.Constants;
using Tsukiy0.Extensions.Http.Exceptions;
using Tsukiy0.Extensions.Http.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Http.Tests.Extensions
{
    public class HttpExtensionsTests
    {
        private static readonly HttpResponseMessage response = new HttpResponseMessage
        {
            RequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://reddit.com")
            },
            StatusCode = HttpStatusCode.OK
        };

        private readonly Mock<ICorrelationService> mockCorrelationService;

        public HttpExtensionsTests()
        {
            mockCorrelationService = new Mock<ICorrelationService>();
        }

        [Fact]
        public void EnsureStatusCode__WhenMatchSuccessCodesThenDoNotThrow()
        {
            Action action = () => response.EnsureStatusCode(new[] { HttpStatusCode.OK });

            action.Should().NotThrow();
        }


        [Fact]
        public void EnsureStatusCode__WhenNotMatchSuccessCodesThenThrow()
        {
            Action action = () => response.EnsureStatusCode(new[] { HttpStatusCode.NotFound });

            action.Should().Throw<HttpException>();
        }

        [Fact]
        public void EnsureStatusCode__DefaultLooksFor200()
        {
            Action action = () => response.EnsureStatusCode();

            action.Should().NotThrow();
        }
    }
}