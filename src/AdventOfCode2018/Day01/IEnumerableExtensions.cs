using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day01
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<TItem> Multiply<TItem>(this IEnumerable<TItem> source, int times)
        {
            return Enumerable
                .Range(0, times)
                .Aggregate(
                    seed: Enumerable.Empty<TItem>(),
                    (prev, _) => prev.Concat(source));
        }
    }
}