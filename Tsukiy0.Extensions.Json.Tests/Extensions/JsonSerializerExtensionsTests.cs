using System.Text.Json;
using FluentAssertions;
using Tsukiy0.Extensions.Json.Extensions;
using Xunit;

namespace Tsukiy0.Extensions.Json.Tests
{
    public class JsonSerializerExtensionsTests
    {
        [Fact]
        public void DefaultJsonSerializerOptions__SerializeCamelcase()
        {
            // Act
            var actual = JsonSerializer.Serialize(new TestCasingClass(1), JsonSerializerExtensions.DefaultJsonSerializerOptions);

            // Assert
            actual.Should().Be("{\"test\":1}");
        }

        [Fact]
        public void DefaultJsonSerializerOptions__DeserializeCamelcase()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestCasingClass>("{\"Test\":1}", JsonSerializerExtensions.DefaultJsonSerializerOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestCasingClass(1));
        }

        [Fact]
        public void DefaultJsonSerializerOptions__DeserializeStringsToNumber()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestCasingClass>("{\"Test\":\"1\"}", JsonSerializerExtensions.DefaultJsonSerializerOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestCasingClass(1));
        }

        [Fact]
        public void DefaultJsonSerializerOptions__SerializeEnumsToString()
        {
            // Act
            var actual = JsonSerializer.Serialize(new
            {
                Enum = TestEnum.BLUE
            }, JsonSerializerExtensions.DefaultJsonSerializerOptions);

            // Assert
            actual.Should().Be("{\"enum\":\"BLUE\"}");
        }

        [Fact]
        public void DefaultJsonSerializerOptions__DeserializeStringToEnum()
        {
            // Act
            var actual = JsonSerializer.Deserialize<TestEnumClass>("{\"enum\":\"BLUE\"}", JsonSerializerExtensions.DefaultJsonSerializerOptions);

            // Assert
            actual.Should().BeEquivalentTo(new TestEnumClass(TestEnum.BLUE));
        }

        private record TestCasingClass(int Test);
        private record TestEnumClass(TestEnum Enum);

        private enum TestEnum
        {
            RED,
            GREEN,
            BLUE
        }
    }
}
