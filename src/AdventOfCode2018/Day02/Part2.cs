namespace AoC18.Day02
{
    internal class Part2
    {
        internal static string GetCommonLetters(string first, string second)
        {
            var s = string.Empty;
            if (first[0] == second[0])
                s += first[0];

            if (first.Length > 1 && first[1] == second[1])
                s += first[1];

            return s;
        }
    }
}