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

        public int Width => claims.Max(c => c.Width);

        public int Height => claims.Max(c => c.Height);
    }
}