using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        private int xOffset;
        private int yOffset;
        private int width;
        private int height;

        public Claim(int xOffset, int yOffset, int width, int height)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.width = width;
            this.height = height;
        }

        internal static Claim Parse(string claim)
        {
            // "#1 @ 1,3: 4x4"
            var tokens = claim.Split(new[] { "@", ",", ":", "x" }, StringSplitOptions.RemoveEmptyEntries);
            // "#1 ", " 1", "3", " 4", "4"
            return new Claim(
                xOffset: int.Parse(tokens[1]),
                yOffset: int.Parse(tokens[2]),
                width: int.Parse(tokens[3]),
                height: int.Parse(tokens[4]));
        }

        internal int this[int x, int y]
        {
            get => x >= xOffset && x < xOffset + width
                && y >= yOffset && y < yOffset + height
                ? 1
                : 0;
        }
    }
}