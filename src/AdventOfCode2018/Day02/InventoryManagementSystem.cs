using System;
using System.Linq;

namespace AoC18.Day02
{
    public static class InventoryManagementSystem
    {
        public const string LineSeparator = "\r\n";

        public static int GetCheckSum(string boxIdsAsString)
        {
            var numbersOfDoubleAndTripleFolds =
                GetBoxIds(boxIdsAsString)
                .Select(GetNumberOfDoubleAndTripleFolds);

            return numbersOfDoubleAndTripleFolds.Sum(t => t.numberOfDoubleFolds)
                * numbersOfDoubleAndTripleFolds.Sum(t => t.numberOfTripleFolds);
        }

        private static string[] GetBoxIds(string boxIdsAsString)
        {
            return boxIdsAsString
                .Split(new[] { LineSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static (int numberOfDoubleFolds, int numberOfTripleFolds) GetNumberOfDoubleAndTripleFolds(string boxId)
        {
            var numbersOfManyFolds = boxId
                .GroupBy(ch => ch)
                .Select(gr => gr.Count())
                .GroupBy(n => n)
                .Select(gr => gr.Key);
            return (numberOfDoubleFolds: numbersOfManyFolds.Count(n => n == 2),
                    numberOfTripleFolds: numbersOfManyFolds.Count(n => n == 3));
        }
    }
}