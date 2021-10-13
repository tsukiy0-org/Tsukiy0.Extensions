using System;

namespace Tsukiy0.Extensions.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? message) : base(message) { }
    }
}
