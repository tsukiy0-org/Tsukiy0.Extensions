using System;

namespace Tsukiy0.Extensions.Data.Models
{
    public interface IDao
    {
        public string Type { get; set; }
        public int Version { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
