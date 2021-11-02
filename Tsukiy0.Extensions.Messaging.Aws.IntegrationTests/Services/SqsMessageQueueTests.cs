using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Polly;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Core.Services;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers;
using Xunit;

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Services
{
    public class SqsMessageQueueTests : IClassFixture<SqsMessageQueueFixture>
    {
        private readonly SqsSaveTestModelQueue _sut;
        private readonly ITestModelRepository _repo;

        public SqsMessageQueueTests(SqsMessageQueueFixture fixture)
        {
            _repo = fixture.Repo;
            _sut = fixture.Sut;
        }

        [Fact]
        public async void Send()
        {
            // Arrange
            var model = new TestModel(Guid.NewGuid(), Guid.NewGuid());

            // Act
            await _sut.Send(new List<SaveTestModelRequest>{
                new SaveTestModelRequest(model)
            });
            var actual = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(12, retryAttempt => TimeSpan.FromSeconds(5))
                .ExecuteAsync(async () =>
                {
                    return (await _repo.QueryByNamespace(model.Namespace)).Single();
                });

            // Assert
            actual.Should().BeEquivalentTo(model);
        }

    }

}
