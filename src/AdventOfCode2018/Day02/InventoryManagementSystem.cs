using System;
using System.Linq;

namespace AoC18.Day02
{
    public static class InventoryManagementSystem
    {
        public const string LineSeparator = "/r/n";

        public static int GetCheckSum(string boxIdsAsString)
        {
            var a = boxIdsAsString
                .Split(new[] { LineSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    var x = s
                        .GroupBy(ch => ch)
                        .Select(gr => gr.Count())
                        .GroupBy(n => n)
                        .Select(gr => gr.Key);
                    return (numberOfDoubleFolds: x.Count(n => n == 2), numberOfTripleFolds: x.Count(n => n == 3));
                });
            var d = a.Sum(t => t.numberOfDoubleFolds);
            var tr = a.Sum(t => t.numberOfTripleFolds);

            return d * tr;
        }
    }
}