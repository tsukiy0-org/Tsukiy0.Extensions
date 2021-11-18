using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tsukiy0.Extensions.Json.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions DefaultOptions
        {
            get
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                options.Converters.Add(new JsonStringEnumConverter());

                return options;
            }
        }
    }
}