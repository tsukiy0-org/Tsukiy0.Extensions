using System.Collections.Generic;
using System.Linq;

namespace Tsukiy0.Extensions.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> list, int size)
        {
            return list.Select((_, i) => new { Value = _, Index = i })
                .GroupBy(_ => _.Index / size)
                .Select(_ => _.Select(_ => _.Value));
        }
    }
}
