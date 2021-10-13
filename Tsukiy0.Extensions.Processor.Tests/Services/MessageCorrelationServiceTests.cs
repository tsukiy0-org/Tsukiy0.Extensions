using System;
using System.Collections.Generic;
using FluentAssertions;
using Tsukiy0.Extensions.Processor.Models;
using Tsukiy0.Extensions.Processor.Services;
using Xunit;

namespace Tsukiy0.Extensions.Processort.Tests.Services
{
    public class MessageCorrelationServiceTests
    {
        private readonly Message<string> message;

        public MessageCorrelationServiceTests()
        {
            message = new Message<string>
            {
                Header = new MessageHeader
                {
                    Version = 1,
                    TraceId = Guid.NewGuid(),
                    Created = DateTimeOffset.MaxValue,
                    AdditionalHeaders = new Dictionary<string, string>()
                },
                Body = "test"
            };
        }

        [Fact]
        public void SpanIdIsRandom()
        {
            // Act
            var actual1 = new MessageCorrelationService(message.Header);
            var actual2 = new MessageCorrelationService(message.Header);

            // Assert
            actual1.SpanId.Should().NotBe(actual2.SpanId);
        }

        [Fact]
        public void WhenHasTraceIdThenReturnThatTraceId()
        {
            // Act
            var actual = new MessageCorrelationService(message.Header);

            // Assert
            actual.TraceId.Should().Be(message.Header.TraceId);
        }

        [Fact]
        public void WhenNoTraceIdThenReturnNewTraceId()
        {
            // Arrange
            message.Header.TraceId = Guid.Empty;

            // Act
            var actual = new MessageCorrelationService(message.Header);

            // Assert
            actual.TraceId.Should().NotBe(Guid.Empty);
        }
    }

}
