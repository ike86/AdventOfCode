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

            for (int i = 0; i < input.Count() / 2; i++)
            {
                if (input[i] == input[i + input.Count() / 2])
                {
                    sum += int.Parse(input[i].ToString());
                }
            }

            return sum;
        }
    }
}