using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
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

    }

}
