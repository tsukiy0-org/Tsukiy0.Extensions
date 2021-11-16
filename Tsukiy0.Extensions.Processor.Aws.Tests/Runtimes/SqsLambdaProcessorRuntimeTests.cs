using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Lambda.SQSEvents;

using AutoFixture;

using FluentAssertions;

using Moq;

using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Messaging.Models;
using Tsukiy0.Extensions.Processor.Aws.Runtimes;
using Tsukiy0.Extensions.Processor.Services;

using Xunit;

using static Amazon.Lambda.SQSEvents.SQSEvent;

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
            var fixture = new Fixture();
            var e = fixture.Build<SQSEvent>()
                .With(_ => _.Records, fixture.Build<SQSMessage>().Without(_ => _.MessageAttributes).CreateMany(10).ToList())
                .Create();

            Func<Task> action = async () => await sut.Run(e);

            await action.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task WhenOneRecordThenProcess()
        {
            // Arrange
            var fixture = new Fixture();
            var message = fixture.Create<Message<string>>();
            var e = fixture.Build<SQSEvent>()
                .With(_ => _.Records, new List<SQSMessage> {
                    new SQSMessage {
                        Body = JsonSerializer.Serialize(message, JsonSerializerExtensions.DefaultOptions)
                    }
                })
                .Create();
            var messages = new List<Message<string>>();
            mockProcessor.Setup(_ => _.Run(Capture.In(messages)));

            // Act
            await sut.Run(e);

            // Assert
            messages.Single().Should().BeEquivalentTo(message);
        }
    }

}