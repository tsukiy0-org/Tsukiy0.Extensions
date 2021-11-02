using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Core.Services;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers;
using Xunit;

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Services
{
    public class BatchMessageQueueTests : IClassFixture<BatchMessageQueueFixture>
    {
        private readonly BatchSaveTestModelQueue _sut;
        private readonly ITestModelRepository _repo;

        public BatchMessageQueueTests(BatchMessageQueueFixture fixture)
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
            await Task.Delay(60000);
            var actual = await _repo.QueryByNamespace(model.Namespace);

            // Assert
            actual.Single().Should().BeEquivalentTo(model);
        }

    }

}
