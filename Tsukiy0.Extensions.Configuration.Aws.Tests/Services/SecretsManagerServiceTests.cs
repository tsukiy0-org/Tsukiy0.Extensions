using System.Collections.Generic;
using System.Text.Json;
using System.Threading;

using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

using FluentAssertions;

using Microsoft.Extensions.Caching.Memory;

using Moq;

using Tsukiy0.Extensions.Configuration.Aws.Services;
using Tsukiy0.Extensions.Json.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Configuration.Aws.Tests.Services
{
    public class SecretsManagerServiceTests
    {
        private readonly Mock<IAmazonSecretsManager> mockClient;
        private readonly SecretsManagerService sut;

        public SecretsManagerServiceTests()
        {
            mockClient = new Mock<IAmazonSecretsManager>();
            sut = new SecretsManagerService(mockClient.Object, new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public async void GetsValue()
        {
            var secret = new Dictionary<string, string>
            {
                ["A"] = "B",
                ["C"] = "D"
            };
            mockClient.Setup(_ => _.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), CancellationToken.None)).ReturnsAsync(new GetSecretValueResponse
            {
                SecretString = JsonSerializer.Serialize(secret, JsonSerializerExtensions.DefaultOptions)
            });

            var actual = await sut.Get("test");

            actual.Should().BeEquivalentTo(secret);
        }

        [Fact]
        public async void WhenGettingMultipleTimesThenUseCache()
        {
            var secret = new Dictionary<string, string>
            {
                ["A"] = "B",
                ["C"] = "D"
            };
            mockClient.Setup(_ => _.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), CancellationToken.None)).ReturnsAsync(new GetSecretValueResponse
            {
                SecretString = JsonSerializer.Serialize(secret, JsonSerializerExtensions.DefaultOptions)
            });

            await sut.Get("test");
            await sut.Get("test");
            await sut.Get("test");
            await sut.Get("test");

            mockClient.Verify(_ => _.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), CancellationToken.None), Times.Once);
        }
    }
}