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

        public static string ToDebugString(this UnionClaim @this, (int x, int y) p)
        {
            var debugString = string.Empty;
            for (int i = 0; i < @this.XOffset + @this.Width; i++)
            {
                for (int j = 0; j < @this.YOffset + @this.Height; j++)
                {
                    if (i == p.x && j == p.y)
                    {
                        debugString += "X";
                    }
                    else
                    {
                        debugString += @this[i, j];
                    }
                }

                debugString += System.Environment.NewLine;
            }

            return debugString;
        }
    }
}