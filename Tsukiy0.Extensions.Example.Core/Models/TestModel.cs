using System;

namespace Tsukiy0.Extensions.Example.Core.Models
{
    public record TestModel
    {
        public Guid Id { get; init; }
        public Guid Namespace { get; init; }
    };
}