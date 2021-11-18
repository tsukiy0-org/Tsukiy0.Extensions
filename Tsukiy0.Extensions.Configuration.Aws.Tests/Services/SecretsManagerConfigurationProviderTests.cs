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
    public class SecretsManagerConfigurationProviderTests
    {
        private readonly Mock<ISecretsManagerService> _mockSecretsManagerService;
        private readonly SecretsManagerConfigurationProvider _sut;

        public SecretsManagerConfigurationProviderTests()
        {
            _mockSecretsManagerService = new Mock<ISecretsManagerService>();
            _sut = new SecretsManagerConfigurationProvider(_mockSecretsManagerService.Object, new List<SecretsManagerMap>
            {
                new SecretsManagerMap{
                    SecretName = "SecretKey",
                    SecretKey = "SecretField",
                    ConfigurationKey = "ConfigKey"
                }
            });
        }

        [Fact]
        public void Load()
        {
            var value = "value";
            _mockSecretsManagerService.Setup(_ => _.Get("SecretKey")).ReturnsAsync(new Dictionary<string, string>
            {
                ["SecretField"] = value
            });

            _sut.Load();
            _sut.TryGet("ConfigKey", out string actual);

            actual.Should().Be(value);
        }

        [Fact]
        public void Load__WhenNoValueThenThrow()
        {
            _mockSecretsManagerService.Setup(_ => _.Get("SecretKey")).ReturnsAsync(new Dictionary<string, string> { });

            Action action = () => _sut.Load();

            action.Should().Throw<SecretNotFoundException>();
        }

    }

}