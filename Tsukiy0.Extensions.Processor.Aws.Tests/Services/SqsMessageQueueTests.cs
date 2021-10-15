using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoFixture;
using Moq;
using Tsukiy0.Extensions.Processor.Aws.Services;
using Tsukiy0.Extensions.Processor.Models;
using Xunit;

namespace Tsukiy0.Extensions.Processor.Aws.Tests.Services
{
    public class SqsMessageQueueTests
    {
        private readonly Mock<IAmazonSQS> mockClient;
        private readonly SqsMessageQueue<string> sut;
        private readonly string queueUrl;

        public SqsMessageQueueTests()
        {
            queueUrl = "test";
            mockClient = new Mock<IAmazonSQS>();
            sut = new SqsMessageQueue<string>(mockClient.Object, queueUrl);
        }

        [Fact]
        public async void Send__ChunksMessages()
        {
            // Arrange
            var messages = new Fixture().CreateMany<Message<string>>(20);

            // Act
            await sut.Send(messages);

            // Assert
            mockClient.Verify(_ => _.SendMessageBatchAsync(It.IsAny<SendMessageBatchRequest>(), CancellationToken.None), Times.Exactly(2));
        }

    }

}
