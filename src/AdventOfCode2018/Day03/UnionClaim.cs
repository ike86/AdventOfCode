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

        public int YOffset => claims[0].YOffset;

        public int Width => claims[0].Width;

        public int Height => claims[0].Height;
    }
}