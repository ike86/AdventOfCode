using System;

namespace AoC18.Day03
{
    internal class Claim
    {
        internal static Claim Parse(string claim)
        {
            return new Claim();
        }

        internal int this[int x, int y]
        {
            get => 0;
        }
    }
}