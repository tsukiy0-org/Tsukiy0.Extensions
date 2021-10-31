using System;
using System.Linq;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tsukiy0.Extensions.Data.Aws.IntegrationTests.Helpers;
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
                .GenerateForever().Take(50).ToList();

            #region PutAll
            // Act
            await _sut.PutAll(models);
            var actualPut = await _sut.QueryByNamespace(ns);

            // Assert
            actualPut.Should().BeEquivalentTo(models);
            #endregion

            #region DeleteAll
            // Act
            await _sut.DeleteAll(models);
            var actualDelete = await _sut.QueryByNamespace(ns);

            // Assert
            actualDelete.Should().BeEmpty();
            #endregion
        }

        [Fact]
        public async void ScanAllAndDeleteAll()
        {
            // Arrange
            var ns = Guid.NewGuid();
            var models = new Faker<TestModel>()
                .CustomInstantiator(f => new TestModel(
                    Id: Guid.NewGuid(),
                    Namespace: ns
                ))
                .GenerateForever().Take(50).ToList();

            #region PutAll
            // Act
            await _sut.PutAll(models);
            var actualPut = await _sut.ScanByNamespace(ns);

            // Assert
            actualPut.Should().BeEquivalentTo(models);
            #endregion

            #region DeleteAll
            // Act
            await _sut.DeleteAll(models);
            var actualDelete = await _sut.ScanByNamespace(ns);

            // Assert
            actualDelete.Should().BeEmpty();
            #endregion
        }
    }
}
