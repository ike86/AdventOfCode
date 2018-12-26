using System;
using System.Linq;

namespace AoC18.Day02
{
    internal class Part2
    {
        internal static string GetCommonLetters(params string[] boxIds)
        {
            foreach (var first in boxIds)
            {
                foreach (var second in boxIds)
                {
                    if (GetCommonLetters(first, second) != string.Empty)
                        return GetCommonLetters(first, second);
                }
            }

            throw new ArgumentException();
        }

        internal static string GetCommonLetters(string first, string second)
        {
            var commonLetters =
                first
                .Zip(
                    second,
                    (x, y) => x == y ? x : default(char?))
                .Where(ch => ch.HasValue)
                .ToArray();

            if (first.Count() - commonLetters.Count() == 1)
            {
                return string.Concat(commonLetters);
            }

            return string.Empty;
        }
    }
}