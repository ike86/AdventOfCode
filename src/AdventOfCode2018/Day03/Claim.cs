using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        private readonly int xOffset;
        private readonly int yOffset;
        private readonly int width;
        private readonly int height;

        public Claim(int xOffset, int yOffset, int width, int height)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.width = width;
            this.height = height;
        }

        internal static Claim Parse(string claim)
        {
            var tokens = claim.Split(
                new[] { "@", ",", ":", "x" },
                StringSplitOptions.RemoveEmptyEntries);

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