using System;
using System.Linq;

namespace AoC18.Day02
{
    public static class InventoryManagementSystem
    {
        public const string LineSeparator = "/r/n";

        public static int GetCheckSum(string boxIdsAsString)
        {
            return boxIdsAsString
                .Split(new[] { LineSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s
                    .GroupBy(ch => ch)
                    .Select(gr => gr.Count())
                    .GroupBy(c => c)
                    .Count(gr => gr.Key > 1))
                .Sum();
        }
    }
}