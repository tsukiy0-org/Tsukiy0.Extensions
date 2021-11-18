namespace Tsukiy0.Extensions.Logging.Models
{
    public record LogException
    {
        public string Type { get; init; }
        public string Message { get; init; }
        public string StackTrace { get; init; }
        public dynamic Context { get; init; }
    };
}