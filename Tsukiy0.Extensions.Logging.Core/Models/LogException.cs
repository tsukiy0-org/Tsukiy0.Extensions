namespace Tsukiy0.Extensions.Logging.Core.Models
{
    public record LogException(
        string Type,
        string Message,
        string StackTrace,
        dynamic Context
    );
}
