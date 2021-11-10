using Tsukiy0.Extensions.Correlation.Services;
using Xunit;
using System;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using FluentAssertions.Extensions;
using Tsukiy0.Extensions.Messaging.Services;
using Moq;
using Tsukiy0.Extensions.Messaging.Models;
using Tsukiy0.Extensions.Testing.Extensions;

namespace Tsukiy0.Extensions.Messaging.Tests.Services
{
    public class DefaultQueueTests
    {
        private readonly TestQueue sut;
        private readonly Mock<IMessageQueue<string>> mockMessageQueue;
        private readonly Mock<ICorrelationService> mockCorrelationService;

        public DefaultQueueTests()
        {
            mockMessageQueue = new Mock<IMessageQueue<string>>();
            mockCorrelationService = new Mock<ICorrelationService>();
            sut = new TestQueue(mockMessageQueue.Object, mockCorrelationService.Object);
        }

        [Fact]
        public async void SendsMessageWithTraceId()
        {
            var traceId = Guid.NewGuid();
            var body = "test";
            mockCorrelationService.Setup(_ => _.TraceId).Returns(traceId);
            var messages = new List<IEnumerable<Message<string>>>();
            mockMessageQueue.Setup(_ => _.Send(Capture.In(messages)));

            await sut.Send(body);

            messages.Single().Single().Should().BeEquivalentTo(new Message<string>
            {
                Header = new MessageHeader
                {
                    Version = 1,
                    TraceId = traceId,
                    Created = DateTimeOffset.UtcNow,
                    AdditionalHeaders = new Dictionary<string, string>()
                },
                Body = body
            },
                o => o
                    .ComparingDateTimesCloseTo(20.Seconds())
            );
        }

        public class TestQueue : DefaultQueue<string>
        {
            public TestQueue(IMessageQueue<string> messageQueue, ICorrelationService correlationService) : base(messageQueue, correlationService)
            {
            }
        }
    }
}
