using System;
using System.Linq;

namespace AdventOfCode2017.Day01
{
    internal class CaptchaInverter2
    {
        private string input;

        public CaptchaInverter2(string input)
        {
            if (input.Count().IsOdd())
            {
                throw new ArgumentException();
            }

            this.input = input;
        }

        internal int GetSum()
        {
            var sum = 0;

            for (int i = 0; i < input.Count(); i++)
            {
                if (input[i] == input[Circulate(Shift(i))])
                {
                    sum += int.Parse(input[Circulate(i)].ToString());
                }
            }

            return sum;
        }

        private int Shift(int i)
        {
            return i + input.Count() / 2;
        }

        private int Circulate(int i)
        {
            return i % input.Count();
        }
    }

    internal static class IntExtensions
    {
        public static bool IsOdd(this int @this)
        {
            return @this % 2 != 0;
        }
    }
}