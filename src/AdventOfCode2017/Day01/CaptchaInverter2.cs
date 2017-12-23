using System;
using System.Linq;

namespace AdventOfCode2017.Day01
{
    internal class CaptchaInverter2
    {
        private string input;

        public CaptchaInverter2(string input)
        {
            if (input.Count() % 2 != 0)
            {
                throw new ArgumentException();
            }

            this.input = input;
        }

        internal int GetSum()
        {
            var sum = 0;

            if (input.Count() > 0 && input[0] == input[1])
            {
                sum += int.Parse(input[0].ToString());
            }

            return sum;
        }
    }
}