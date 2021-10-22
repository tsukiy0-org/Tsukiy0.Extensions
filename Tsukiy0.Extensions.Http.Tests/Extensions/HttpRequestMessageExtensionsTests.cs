using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentAssertions;
using Moq;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Http.Extensions;
using Xunit;

namespace Tsukiy0.Extensions.Http.Tests.Extensions
{
    public class HttpRequestMessageExtensionsTests
    {
        private readonly HttpRequestMessage _sample;

        public HttpRequestMessageExtensionsTests()
        {
            _sample = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://example.com")
            };
        }

        [Fact]
        public void AddBasicAuth()
        {
            _sample.AddBasicAuth("username", "password");
            _sample.Headers.TryGetValues("Authorization", out IEnumerable<string> actual);

            actual.Should().HaveCount(1);
            actual.Single().Should().Be("Basic dXNlcm5hbWU6cGFzc3dvcmQ=");
        }

        [Fact]
        public void AddUserAgent()
        {
            _sample.AddUserAgent(new ProductInfoHeaderValue("Test", "1.0"));
            _sample.Headers.TryGetValues("User-Agent", out IEnumerable<string> actual);

            actual.Should().HaveCount(1);
            actual.Single().Should().Be("Test/1.0");
        }

        [Fact]
        public void AddHeader()
        {
            var key = "X-Test";
            var value = "Value";

            _sample.AddHeader(key, value);
            _sample.Headers.TryGetValues(key, out IEnumerable<string> actual);

            actual.Should().HaveCount(1);
            actual.Single().Should().Be(value);
        }

        [Fact]
        public void AddTraceId__AddsTraceIdToHeader()
        {
            var traceId = Guid.NewGuid();
            var mockCorrelationService = new Mock<ICorrelationService>();
            mockCorrelationService.Setup(_ => _.TraceId).Returns(traceId);

            _sample.AddTraceId(mockCorrelationService.Object);
            _sample.Headers.TryGetValues(Constants.HttpHeaders.TraceId, out IEnumerable<string> actual);

            actual.Should().HaveCount(1);
            actual.Single().Should().Be(traceId.ToString());
        }

        [Fact]
        public void AddApiKey__AddsApiKeyToHeader()
        {
            var key = Guid.NewGuid().ToString();

            _sample.AddApiKey(key);
            _sample.Headers.TryGetValues(Constants.HttpHeaders.ApiKey, out IEnumerable<string> actual);

            actual.Should().HaveCount(1);
            actual.Single().Should().Be(key.ToString());
        }
    }
}
