using System;

namespace Tsukiy0.Extensions.Data.Models
{
    public interface IDao
    {
        public string __Type { get; set; }
        public int __Version { get; set; }
        public DateTimeOffset __Updated { get; set; }
    }
}
