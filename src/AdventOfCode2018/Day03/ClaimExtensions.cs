using static System.Math;

namespace AoC18.Day03
{
    internal static class ClaimExtensions
    {
        public static int OrHorizontallyInboundOf(this int x, Claim claim)
        {
            return
                Min(
                    Max(x, claim.XOffset),
                    claim.XOffset + claim.Width);
        }

        public static int OrVerticallyInboundOf(this int y, Claim claim)
        {
            return
                Min(
                    Max(y, claim.YOffset),
                    claim.YOffset + claim.Height);
        }
    }
}