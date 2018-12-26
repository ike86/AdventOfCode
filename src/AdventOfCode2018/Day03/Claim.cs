using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        public Claim(int xOffset, int yOffset, int width, int height)
        {
            XOffset = xOffset;
            YOffset = yOffset;
            Width = width;
            Height = height;
        }

        public int XOffset { get; }

        public int YOffset { get; }

        public int Width { get; }

        public int Height { get; }

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
            get => x >= XOffset && x < XOffset + Width
                && y >= YOffset && y < YOffset + Height
                ? 1
                : 0;
        }
    }
}