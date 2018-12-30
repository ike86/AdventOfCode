using System;

namespace AoC18.Day03
{
    public class Claim
    {
        public Claim((int x, int y) topleft, (int x, int y) bottomRight)
        {
            XOffset = topleft.x;
            YOffset = topleft.y;
            BottomRightX = bottomRight.x;
            BottomRightY = bottomRight.y;
        }

        public Claim(int id, int xOffset, int yOffset, int width, int height)
            : this(
                 topleft: (x: xOffset, y: yOffset),
                 bottomRight: (x: xOffset + width, y: yOffset + height))
        {
            Id = id;
        }

        public int XOffset { get; }

        public int YOffset { get; }

        public int Width => BottomRightX - XOffset;

        public int Height => BottomRightY - YOffset;

        public int BottomRightX { get; }

        public int BottomRightY { get; }
        public int Id { get; }

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
                new[] { "#", "@", ",", ":", "x" },
                StringSplitOptions.RemoveEmptyEntries);

            return new Claim(
                id: int.Parse(tokens[0]),
                xOffset: int.Parse(tokens[1]),
                yOffset: int.Parse(tokens[2]),
                width: int.Parse(tokens[3]),
                height: int.Parse(tokens[4]));
        }
    }
}