using System;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tsukiy0.Extensions.Logging.Extensions;
using Xunit;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Logging.NLog.Tests.Helpers;

namespace Tsukiy0.Extensions.Logging.NLog.Tests
{
    public class LogLayoutRendererTests
    {
        private readonly MemoryLoggerHelper<LogLayoutRendererTests> helper;

        public LogLayoutRendererTests()
        {
            helper = new MemoryLoggerHelper<LogLayoutRendererTests>();
        }

        [Fact]
        public void LogInformation()
        {
            helper.Logger.LogInformation("Hello {p1} {p2}", "param1", new { Complex = "Object" });
            var actual = helper.GetFirstLog();

            actual.Version.Should().Be(1);
            actual.Level.Should().Be(20);
            actual.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, 200);
            actual.Name.Should().Be("TestApp");
            actual.TraceId.Should().BeNull();
            actual.SpanId.Should().BeNull();
            actual.Message.Should().Contain("Hello");
            actual.Message.Should().Contain("param1");
            actual.Message.Should().Contain("Complex");
            actual.Message.Should().Contain("Object");
            actual.Exception.Should().BeNull();
            var context = (JsonElement)actual.Context;
            context.GetProperty("p1").GetString().Should().Be("param1");
            context.GetProperty("p2").GetProperty("Complex").GetString().Should().Be("Object");
        }

        [Fact]
        public void LogError()
        {
            var exception = new Exception("Something Bad!");
            exception.Data.Add("Errors", "Are Bad");
            exception.Data.Add("ComplexErrors", new { Value = "Are Worse" });
            helper.Logger.LogError(exception, "Hello {p1} {p2}", "param1", new { Complex = "Object" });
            var actual = helper.GetFirstLog();

            actual.Version.Should().Be(1);
            actual.Level.Should().Be(40);
            actual.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, 200);
            actual.Name.Should().Be("TestApp");
            actual.TraceId.Should().BeNull();
            actual.SpanId.Should().BeNull();
            actual.Message.Should().Contain("Hello");
            actual.Message.Should().Contain("param1");
            actual.Message.Should().Contain("Complex");
            actual.Message.Should().Contain("Object");
            var context = (JsonElement)actual.Context;
            context.GetProperty("p1").GetString().Should().Be("param1");
            context.GetProperty("p2").GetProperty("Complex").GetString().Should().Be("Object");
            actual.Exception.Message.Should().Equals(exception.Message);
            actual.Exception.Type.Should().Equals(exception.GetType().Name);
            actual.Exception.StackTrace.Should().Equals(exception.StackTrace);
            var exceptionContext = (JsonElement)actual.Exception.Context;
            exceptionContext.GetProperty("Errors").GetString().Should().Be("Are Bad");
            exceptionContext.GetProperty("ComplexErrors").GetProperty("Value").GetString().Should().Be("Are Worse");
        }

        [Fact]
        public void WithScope()
        {
            var correlationService = new StaticCorrelationService(Guid.NewGuid(), Guid.NewGuid());

            helper.Logger.WithCorrelation(correlationService, () =>
            {
                helper.Logger.LogInformation("Hello {p1} {p2}", "param1", new { Complex = "Object" });
                var actual = helper.GetFirstLog();

                actual.TraceId.Should().Equals(correlationService.TraceId);
                actual.SpanId.Should().Equals(correlationService.SpanId);
            });
        }
    }
}
