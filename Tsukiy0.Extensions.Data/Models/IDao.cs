using System;

namespace Tsukiy0.Extensions.Data.Models
{
    public interface IDao
    {
        public string __TYPE { get; }
        public int __VERSION { get; }
        public DateTimeOffset __UPDATED { get; }
    }
}
