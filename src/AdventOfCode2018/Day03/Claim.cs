using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        private int xOffset;

        public Claim(int xOffset)
        {
            this.xOffset = xOffset;
        }

        internal static Claim Parse(string claim)
        {
            // "#1 @ 1,3: 4x4"
            var tokens = claim.Split(new[] { "@", "," }, StringSplitOptions.RemoveEmptyEntries);
            // "#1 ", " 1", "3: 4x4"
            return new Claim(xOffset: int.Parse(tokens[1]));
        }

        internal int this[int x, int y]
        {
            get => x >= xOffset ? 1 : 0;
        }
    }
}