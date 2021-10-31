using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.IntegrationTests.Helpers;
using Tsukiy0.Extensions.Data.Aws.Models;
using Xunit;

namespace Tsukiy0.Extensions.Data.Aws.IntegrationTests.Extensions
{
    public partial class DynamoExtensionsTests
    {
        private readonly TestModelDynamoRepository _sut;

        public DynamoExtensionsTests()
        {
            var host = TestHostBuilder.Build();
            _sut = host.Services.GetRequiredService<TestModelDynamoRepository>();
        }

        [Fact]
        public async void PutAllAndDeleteAll()
        {
            // Arrange
            var ns = Guid.NewGuid();
            var models = new Faker<TestModel>()
                .CustomInstantiator(f => new TestModel(
                    Id: Guid.NewGuid(),
                    Namespace: ns
                ))
                .GenerateForever().Take(100);

            #region PutAll
            // Act
            await _sut.PutAll(models);
            var actualPut = await _sut.ListByNamespace(ns);

            // Assert
            actualPut.Should().BeEquivalentTo(models);
            #endregion

            #region DeleteAll
            // Act
            await _sut.DeleteAll(models);
            var actualDelete = await _sut.ListByNamespace(ns);

            // Assert
            actualDelete.Should().BeEmpty();
            #endregion

        }
    }
}
