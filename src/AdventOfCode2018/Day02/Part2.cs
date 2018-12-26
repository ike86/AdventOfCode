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
                .Where(ch => ch.HasValue);

            return string.Concat(commonLetters);
        }
    }
}