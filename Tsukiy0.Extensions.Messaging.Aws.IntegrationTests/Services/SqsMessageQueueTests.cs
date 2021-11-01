using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers;
using Xunit;

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Services
{
    public class SqsMessageQueueTests
    {
        private readonly SqsSaveTestModelQueue _sut;
        private readonly DynamoTestModelRepository _repo;

        public SqsMessageQueueTests()
        {
            var host = TestHostBuilder.Build();
            _repo = host.Services.GetRequiredService<DynamoTestModelRepository>();
            _sut = host.Services.GetRequiredService<SqsSaveTestModelQueue>();
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
            await Task.Delay(30000);
            var actual = await _repo.QueryByNamespace(model.Namespace);

            // Assert
            actual.Single().Should().BeEquivalentTo(model);
        }

    }

}
