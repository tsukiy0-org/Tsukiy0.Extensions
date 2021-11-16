using System.Text.Json;

using FluentAssertions;

using Tsukiy0.Extensions.Json.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Json.Tests
{
    public class JsonSerializerExtensionsTests
    {
        [Fact]
        public void DefaultOptions__SerializeCamelcase()
        {
            // Act
            var actual = JsonSerializer.Serialize(new TestCasingClass { Test = 1 }, JsonSerializerExtensions.DefaultOptions);

            // Assert
            actual.Should().Be("{\"test\":1}");
        }

        [Fact]
        public void DefaultOptions__DeserializeCamelcase()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestCasingClass>("{\"test\":1}", JsonSerializerExtensions.DefaultOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestCasingClass { Test = 1 });
        }

        [Fact]
        public void DefaultOptions__DeserializePascalcase()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestCasingClass>("{\"Test\":1}", JsonSerializerExtensions.DefaultOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestCasingClass { Test = 1 });
        }

        [Fact]
        public void DefaultOptions__DeserializeStringsToNumber()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestCasingClass>("{\"Test\":\"1\"}", JsonSerializerExtensions.DefaultOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestCasingClass { Test = 1 });
        }

        [Fact]
        public void DefaultOptions__SerializeEnumsToString()
        {
            // Act
            var actual = JsonSerializer.Serialize(new
            {
                Enum = TestEnum.BLUE
            }, JsonSerializerExtensions.DefaultOptions);

            // Assert
            actual.Should().Be("{\"enum\":\"BLUE\"}");
        }

        [Fact]
        public void DefaultOptions__DeserializeStringToEnum()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestEnumClass>("{\"test\":\"BLUE\"}", JsonSerializerExtensions.DefaultOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestEnumClass { Test = TestEnum.BLUE });
        }

        private record TestCasingClass
        {
            public int Test { get; init; }
        }
        private record TestEnumClass
        {
            public TestEnum Test { get; init; }
        };

        private enum TestEnum
        {
            RED,
            GREEN,
            BLUE
        }
    }
}