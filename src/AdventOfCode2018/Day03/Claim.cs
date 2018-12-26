using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        private int xOffset;
        private int yOffset;
        private int width;

        public Claim(int xOffset, int yOffset, int width)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.width = width;
        }

        internal static Claim Parse(string claim)
        {
            // "#1 @ 1,3: 4x4"
            var tokens = claim.Split(new[] { "@", ",", ":", "x" }, StringSplitOptions.RemoveEmptyEntries);
            // "#1 ", " 1", "3", " 4", "4"
            return new Claim(
                xOffset: int.Parse(tokens[1]),
                yOffset: int.Parse(tokens[2]),
                width: int.Parse(tokens[3]));
        }

        internal int this[int x, int y]
        {
            get => x >= xOffset && x < xOffset + width
                && y >= yOffset
                ? 1
                : 0;
        }
    }
}