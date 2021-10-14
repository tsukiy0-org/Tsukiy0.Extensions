using System;

namespace Tsukiy0.Extensions.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string? message) : base(message) { }
    }
}
