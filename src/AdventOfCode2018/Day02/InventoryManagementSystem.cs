using System.Linq;

namespace AoC18.Day02
{
    public static class InventoryManagementSystem
    {
        public static int GetCheckSum(string boxIdsAsString)
        {
            return boxIdsAsString
                .GroupBy(ch => ch)
                .Count(gr => gr.Count() > 1);
        }
    }
}