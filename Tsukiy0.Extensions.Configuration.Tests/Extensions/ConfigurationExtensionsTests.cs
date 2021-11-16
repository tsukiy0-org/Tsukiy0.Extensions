using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Tsukiy0.Extensions.Configuration.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Configuration.Tests.Extensions
{
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void GetSection_ByType()
        {
            //Arange
            var configuration = SetupConfiguration(new Dictionary<string, string>
            {
                {"TestConfig:A", "1"},
                {"TestConfig:B", "2"}
            });

            //Act
            var config = configuration.GetSection<TestConfig>();

            //Assert
            config.Should().BeEquivalentTo(new TestConfig
            {
                A = "1",
                B = "2"
            });
        }

        [Fact]
        public void GetSection_ByName()
        {
            //Arange
            var configuration = SetupConfiguration(new Dictionary<string, string>
            {
                {"SomethingRandom:A", "1"},
                {"SomethingRandom:B", "2"}
            });

            //Act
            var config = configuration.GetSection<TestConfig>("SomethingRandom");

            //Assert
            config.Should().BeEquivalentTo(new TestConfig
            {
                A = "1",
                B = "2"
            });
        }

        [Fact]
        public void GetSection_WhenRecordThenFailsDueToNoConstructor()
        {
            //Arange
            var configuration = SetupConfiguration(new Dictionary<string, string>
            {
                {"TestRecordConfig:A", "1"},
                {"TestRecordConfig:B", "2"}
            });

            //Act
            Action action = () => configuration.GetSection<TestRecordConfig>();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        private class TestConfig
        {
            public string A { get; init; }
            public string B { get; init; }
        }

        private record TestRecordConfig(string A, string B);

        private IConfiguration SetupConfiguration(IDictionary<string, string> config)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();
        }
    }
}