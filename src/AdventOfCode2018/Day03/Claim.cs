using System;

namespace AoC18.Day03
{
    public class Claim
    {
        private (int, int) a1;
        private (int, int) a2;

        public Claim(int xOffset, int yOffset, int width, int height)
        {
            XOffset = xOffset;
            YOffset = yOffset;
            Width = width;
            Height = height;
        }

        public Claim((int, int) a1, (int, int) a2)
        {
            XOffset = a1.Item1;
            YOffset = a1.Item2;
            Width = a2.Item1 - a1.Item1;
            Height = a2.Item2 - a1.Item2;
        }

        public int XOffset { get; }

        public int YOffset { get; }

        public int Width { get; }

        public int Height { get; }

        internal int this[int x, int y]
        {
            get => x >= XOffset && x < XOffset + Width
                && y >= YOffset && y < YOffset + Height
                ? 1
                : 0;
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
    }
}