using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Tsukiy0.Extensions.Configuration.Aws.Services;
using Xunit;
using static Tsukiy0.Extensions.Configuration.Aws.Services.SsmParameterService;

namespace Tsukiy0.Extensions.Configuration.Aws.Tests.Services
{
    public class SsmParameterServiceTests
    {
        private readonly Mock<IAmazonSimpleSystemsManagement> mockSsmClient;
        private readonly SsmParameterService sut;

        public SsmParameterServiceTests()
        {
            mockSsmClient = new Mock<IAmazonSimpleSystemsManagement>();
            sut = new SsmParameterService(mockSsmClient.Object, new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public async void WhenNotExistsThenReturnsNull()
        {
            mockSsmClient.Setup(_ => _.GetParameterAsync(It.IsAny<GetParameterRequest>(), CancellationToken.None)).ReturnsAsync(new GetParameterResponse
            {
                Parameter = new Parameter
                {
                    Value = null
                }
            });

            var actual = await sut.Get("test");

            actual.Should().BeNull();
        }

        [Fact]
        public async void GetsValue()
        {
            var value = "value";
            mockSsmClient.Setup(_ => _.GetParameterAsync(It.IsAny<GetParameterRequest>(), CancellationToken.None)).ReturnsAsync(new GetParameterResponse
            {
                Parameter = new Parameter
                {
                    Value = value
                }
            });

            var actual = await sut.Get("test");

            actual.Should().Be(value);
        }

        [Fact]
        public async void WhenGettingMultipleTimesThenUseCache()
        {
            var value = "value";
            mockSsmClient.Setup(_ => _.GetParameterAsync(It.IsAny<GetParameterRequest>(), CancellationToken.None)).ReturnsAsync(new GetParameterResponse
            {
                Parameter = new Parameter
                {
                    Value = value
                }
            });

            await sut.Get("test");
            await sut.Get("test");
            await sut.Get("test");
            await sut.Get("test");

            mockSsmClient.Verify(_ => _.GetParameterAsync(It.IsAny<GetParameterRequest>(), CancellationToken.None), Times.Once);
        }
    }
}
