using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.AspNetCore.Filters;
using Xunit;

namespace Tsukiy0.Extensions.AspNetCore.Tests.Filters
{
    public class ApiKeyAuthFilterTests
    {
        private readonly ApiKeyAuthConfig config = new ApiKeyAuthConfig
        {
            ApiKeys = new Dictionary<string, string>{
                { "Operations", Guid.NewGuid().ToString() },
                { "Service", Guid.NewGuid().ToString() },
            }
        };
        private readonly AuthorizationFilterContext context;
        private readonly ApiKeyAuthFilter sut;

        public ApiKeyAuthFilterTests()
        {
            sut = new ApiKeyAuthFilter(config);
            context = new AuthorizationFilterContext(
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
        public void WhenHasKey__ThenPass(string keyName)
        {
            context.HttpContext.Request.Headers.Add(ApiKeyAuthFilter.Header, config.ApiKeys[keyName]);

            sut.OnAuthorization(context);

            context.Result.Should().BeNull();
        }

        [Fact]
        public void WhenNoKey__ThenUnauthorized()
        {
            sut.OnAuthorization(context);

            context.Result.Should().BeEquivalentTo(new UnauthorizedResult());
        }

        [Fact]
        public void WhenNotMatchingKey__ThenUnauthorized()
        {
            context.HttpContext.Request.Headers.Add(ApiKeyAuthFilter.Header, Guid.NewGuid().ToString());

            sut.OnAuthorization(context);

            context.Result.Should().BeEquivalentTo(new UnauthorizedResult());
        }
    }
}
