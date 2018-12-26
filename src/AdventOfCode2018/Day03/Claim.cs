using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        private int xOffset;
        private int yOffset;

        public Claim(int xOffset, int yOffset)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }

        internal static Claim Parse(string claim)
        {
            // "#1 @ 1,3: 4x4"
            var tokens = claim.Split(new[] { "@", ",", ":" }, StringSplitOptions.RemoveEmptyEntries);
            // "#1 ", " 1", "3", " 4x4"
            return new Claim(
                xOffset: int.Parse(tokens[1]),
                yOffset: int.Parse(tokens[2]));
        }

        internal int this[int x, int y]
        {
            get => x >= xOffset && y >= yOffset ? 1 : 0;
        }
    }
}