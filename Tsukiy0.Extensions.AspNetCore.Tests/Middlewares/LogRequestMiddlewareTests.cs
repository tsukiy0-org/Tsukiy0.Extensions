using Moq;
using Xunit;
using FluentAssertions;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Tsukiy0.Extensions.Logging.NLog.Tests.Helpers;
using Tsukiy0.Extensions.Logging.AspNetCore.Middlewares;

namespace Tsukiy0.Extensions.AspNetCore.Tests.Middlewares
{
    public class LogRequestMiddlewareTests
    {
        private readonly MemoryLoggerHelper<LogRequestMiddleware> helper;
        private readonly LogRequestMiddleware sut;

        public LogRequestMiddlewareTests()
        {
            helper = new MemoryLoggerHelper<LogRequestMiddleware>();
            sut = new LogRequestMiddleware(helper.Logger);
        }

        [Fact]
        public async void LogsRequestAndResponse()
        {
            var mockRequestDelegate = new Mock<RequestDelegate>();
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("google.com");
            context.Request.Scheme = "http";
            context.Request.Protocol = "HTTP/1.1";
            context.Request.Method = "POST";
            context.Request.Path = "/WeatherForecast";
            context.Request.QueryString = new QueryString("?test");
            context.Request.ContentType = "application/json";
            context.Request.ContentLength = 100;
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/xml";
            context.Response.ContentLength = 300;

            await sut.InvokeAsync(context, mockRequestDelegate.Object);
            var actual = helper.GetFirstLog();

            JsonSerializer.Serialize((object)actual.Context).Should().Be(JsonSerializer.Serialize(
                new
                {
                    Request = new
                    {
                        Host = "google.com",
                        Scheme = "http",
                        Protocol = "HTTP/1.1",
                        Method = "POST",
                        Path = "/WeatherForecast",
                        Query = "?test",
                        ContentType = "application/json",
                        ContentLength = 100
                    },
                    Response = new
                    {
                        StatusCode = 200,
                        ContentType = "application/xml",
                        ContentLength = 300
                    }
                }
            ));
        }
    }
}
