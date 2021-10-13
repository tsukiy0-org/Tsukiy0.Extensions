using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using Moq;
using Tsukiy0.Extensions.Processor.Services;
using Tsukiy0.Extensions.Processor.Aws.Runtimes;
using FluentAssertions;
using FizzWare.NBuilder;
using Amazon.Lambda.SQSEvents;
using Tsukiy0.Extensions.Processor.Models;
using static Amazon.Lambda.SQSEvents.SQSEvent;
using System.Linq;

namespace Tsukiy0.Extensions.Processor.Aws.Tests.Runtimes
{
    public class SqsLambdaProcessorRuntimeTests
    {
        private readonly Mock<IProcessor<string>> mockProcessor;
        private readonly SqsLambdaProcessorRuntime<string> sut;

        public SqsLambdaProcessorRuntimeTests()
        {
            mockProcessor = new Mock<IProcessor<string>>();
            sut = new SqsLambdaProcessorRuntime<string>(mockProcessor.Object);
        }

        [Fact]
        public async Task WhenMoreThanOneRecordThenThrow()
        {
            var e = Builder<SQSEvent>.CreateNew()
                .With(_ => _.Records = new List<SQSMessage> {
                    Builder<SQSMessage>.CreateNew().Build(),
                    Builder<SQSMessage>.CreateNew().Build(),
                })
                .Build();

            Func<Task> action = async () => await sut.Run(e);

            await action.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task WhenOneRecordThenProcess()
        {
            // Arrange
            var message = new Message<string>
            (
                Header: new MessageHeader
                (
                    Version: 1,
                    TraceId: Guid.NewGuid(),
                    Created: DateTimeOffset.MaxValue,
                    AdditionalHeaders: new Dictionary<string, string>()
                ),
                Body: "yay!"
            );
            var e = Builder<SQSEvent>.CreateNew()
                .With(_ => _.Records = new List<SQSMessage> {
                    Builder<SQSMessage>.CreateNew()
                        .With(_ => _.Body = JsonSerializer.Serialize(message))
                        .Build(),
                })
                .Build();
            var messages = new List<Message<string>>();
            mockProcessor.Setup(_ => _.Run(Capture.In(messages)));

            // Act
            await sut.Run(e);

            // Assert
            messages.Single().Should().BeEquivalentTo(message);
        }
    }

}
