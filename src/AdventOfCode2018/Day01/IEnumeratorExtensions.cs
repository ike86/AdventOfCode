using System.Collections.Generic;

namespace AoC18.Day01
{
    internal static class IEnumeratorExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}