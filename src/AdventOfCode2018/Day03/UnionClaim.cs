using System.Linq;

namespace AoC18.Day03
{
    internal class UnionClaim
    {
        private Claim[] claims;

        public UnionClaim(params Claim[] claims)
        {
            this.claims = claims;
        }

        public int XOffset => claims.Min(c => c.XOffset);

        public int YOffset => claims.Min(c => c.YOffset);

        public int Width => BottomRightX - XOffset;

        public int Height => claims.Max(c => c.Height);

        public int BottomRightX => claims.Max(c => c.BottomRightX);

        public int BottomRightY => claims.Max(c => c.BottomRightY);

        internal int this[int x, int y] => claims.Sum(c => c[x, y]);
    }
}