using System;
using System.Collections.Generic;

using FluentAssertions;

using Moq;

using Tsukiy0.Extensions.Configuration.Aws.Exceptions;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Configuration.Aws.Services;

using Xunit;

namespace Tsukiy0.Extensions.Configuration.Aws.Tests.Services
{
    public class SsmParameterConfigurationProviderTests
    {
        private readonly Mock<ISsmParameterService> _mockSsmParameterService;
        private readonly SsmParameterConfigurationProvider _sut;

        public SsmParameterConfigurationProviderTests()
        {
            _mockSsmParameterService = new Mock<ISsmParameterService>();
            _sut = new SsmParameterConfigurationProvider(_mockSsmParameterService.Object, new List<SsmParameterMap>
            {
                new SsmParameterMap{
                    ParameterKey = "SsmKey",
                    ConfigurationKey = "ConfigKey"
                }
            });
        }

        [Fact]
        public void Load()
        {
            var value = "value";
            _mockSsmParameterService.Setup(_ => _.Get("SsmKey")).ReturnsAsync(value);

            _sut.Load();
            _sut.TryGet("ConfigKey", out string actual);

            actual.Should().Be(value);
        }

        [Fact]
        public void Load__WhenNoValueThenThrow()
        {
            _mockSsmParameterService.Setup(_ => _.Get("SsmKey")).ReturnsAsync((string)null);

            Action action = () => _sut.Load();

            action.Should().Throw<SsmParameterNotFoundException>();
        }

    }

}