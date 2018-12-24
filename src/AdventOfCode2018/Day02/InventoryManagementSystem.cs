using System.Linq;

namespace AoC18.Day02
{
    public static class InventoryManagementSystem
    {
        public static int GetCheckSum(string boxIdsAsString)
        {
            var a = boxIdsAsString
                .GroupBy(ch => ch)
                .Select(gr => gr.Count())
                .GroupBy(c => c);
            return a.Count(gr => gr.Key > 1);
        }
    }
}