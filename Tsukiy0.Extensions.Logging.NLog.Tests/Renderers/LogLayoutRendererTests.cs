using System;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using Tsukiy0.Extensions.NLog.Renderers;
using Tsukiy0.Extensions.Logging.Core.Extensions;
using Tsukiy0.Extensions.Logging.Core.Models;
using Tsukiy0.Extensions.Logging.Core.Services;
using Xunit;

namespace Tsukiy0.Extensions.Logging.NLog.Tests
{
    public class LogLayoutRendererTests
    {
        private readonly MemoryTarget target;
        private readonly Microsoft.Extensions.Logging.ILogger sut;

        public LogLayoutRendererTests()
        {
            var config = new LoggingConfiguration();
            LayoutRenderer.Register("shared-log", _ =>
            {
                var renderer = new LogLayoutRenderer("TestApp");
                return renderer.Render(_);
            });
            target = new MemoryTarget()
            {
                Layout = Layout.FromString("${standard-log}")
            };
            config.AddRuleForAllLevels(target);
            LogManager.Configuration = config;
            var provider = new NLogLoggerProvider();
            sut = provider.CreateLogger("test");
        }

        [Fact]
        public void LogInformation()
        {
            sut.LogInformation("Hello {p1} {p2}", "param1", new { Complex = "Object" });
            var actual = JsonSerializer.Deserialize<Log>(target.Logs[0]);

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
            sut.LogError(exception, "Hello {p1} {p2}", "param1", new { Complex = "Object" });
            var actual = JsonSerializer.Deserialize<Log>(target.Logs[0]);

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

            sut.WithCorrelation(correlationService, () =>
            {
                sut.LogInformation("Hello {p1} {p2}", "param1", new { Complex = "Object" });
                var actual = JsonSerializer.Deserialize<Log>(target.Logs[0]);

                actual.TraceId.Should().Equals(correlationService.TraceId);
                actual.SpanId.Should().Equals(correlationService.SpanId);
            });
        }
    }
}
