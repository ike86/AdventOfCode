namespace AoC18.Day02
{
    internal class Part2
    {
        internal static string GetCommonLetters(string first, string second)
        {
            var commonLetters = string.Empty;
            if (first[0] == second[0])
                commonLetters += first[0];

            if (first.Length > 1 && first[1] == second[1])
                commonLetters += first[1];

            if (first.Length > 2 && first[2] == second[2])
                commonLetters += first[2];

            return commonLetters;
        }
    }
}