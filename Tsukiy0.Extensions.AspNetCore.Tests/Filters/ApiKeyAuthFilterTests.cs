using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using Moq;

using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.AspNetCore.Filters;
using Tsukiy0.Extensions.Http.Constants;

using Xunit;

namespace Tsukiy0.Extensions.AspNetCore.Tests.Filters
{
    public class ApiKeyAuthFilterTests
    {
        private readonly ApiKeyAuthConfig _config = new ApiKeyAuthConfig
        {
            ApiKeys = new Dictionary<string, string>{
                { "Operations", Guid.NewGuid().ToString() },
                { "Service", Guid.NewGuid().ToString() },
            }
        };
        private readonly AuthorizationFilterContext _context;
        private readonly ApiKeyAuthFilter _sut;

        public ApiKeyAuthFilterTests()
        {
            var logger = new Mock<ILogger<ApiKeyAuthFilter>>();
            _sut = new ApiKeyAuthFilter(_config, logger.Object);
            _context = new AuthorizationFilterContext(
                new ActionContext()
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                },
                new List<IFilterMetadata>()
            );
        }

        [Theory]
        [InlineData("Operations")]
        [InlineData("Service")]
        public void WhenHasKeyThenPass(string keyName)
        {
            _context.HttpContext.Request.Headers.Add(HttpHeaders.ApiKey, _config.ApiKeys[keyName]);

            _sut.OnAuthorization(_context);

            _context.Result.Should().BeNull();
        }

        [Fact]
        public void WhenNoKeyThenUnauthorized()
        {
            _sut.OnAuthorization(_context);

            _context.Result.Should().BeEquivalentTo(new UnauthorizedResult());
        }

        [Fact]
        public void WhenNotMatchingKeyThenUnauthorized()
        {
            _context.HttpContext.Request.Headers.Add(HttpHeaders.ApiKey, Guid.NewGuid().ToString());

            _sut.OnAuthorization(_context);

            _context.Result.Should().BeEquivalentTo(new UnauthorizedResult());
        }
    }
}