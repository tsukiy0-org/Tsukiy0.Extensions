using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using FluentAssertions;
using Tsukiy0.Extensions.Aws.Core.Models;
using Tsukiy0.Extensions.Aws.Core.Services;
using Tsukiy0.Extensions.Aws.Infrastructure.Services;
using Xunit;

namespace Tsukiy0.Extensions.Aws.Infrastructure.IntegrationTests.Services
{
    public class SqsClientTests
    {
        private readonly ISqsClient<TestMessage> sut;

        public SqsClientTests()
        {
            var queueUrl = Environment.GetEnvironmentVariable("SqsClientQueueUrl");
            sut = new SqsClient<TestMessage>(new AmazonSQSClient(RegionEndpoint.USEast1), new Uri(queueUrl));
        }

        private record TestMessage(string Header, string Body);

        [Fact]
        public async Task SendOneMessage()
        {
            var sendMessage = new SendMessageEnvelope<TestMessage>(
                Guid.NewGuid(),
                new TestMessage("header_test", "body_test")
            );

            await sut.Send(new[] { sendMessage });
            var actual = await sut.Receive();

            actual.ReceiptHandle.Should().NotBeNull();
            actual.TraceId.Should().Be(sendMessage.TraceId);
            actual.Message.Should().BeEquivalentTo(sendMessage.Message);

            await sut.Delete(actual.ReceiptHandle);
        }
    }
}
