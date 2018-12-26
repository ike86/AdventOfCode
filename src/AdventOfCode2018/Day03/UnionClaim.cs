namespace AoC18.Day03
{
    internal class UnionClaim
    {
        private Claim claim;

        public UnionClaim(Claim claim)
        {
            this.claim = claim;
        }

        public int XOffset => claim.XOffset;

        public int YOffset => claim.YOffset;

        public int Width => claim.Width;

        public int Height => claim.Height;
    }
}