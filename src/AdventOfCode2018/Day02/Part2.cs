using System.Linq;

namespace AoC18.Day02
{
    internal class Part2
    {
        internal static string GetCommonLetters(string first, string second)
        {
            var commonLetters =
                first
                .Zip(
                    second,
                    (x, y) => x == y ? x : default(char?))
                .ToArray();
            if (commonLetters.Count(ch => !ch.HasValue) > 1)
                return string.Empty;

            commonLetters = commonLetters
                .Where(ch => ch.HasValue)
                .ToArray();

            return string.Concat(commonLetters);
        }
    }
}