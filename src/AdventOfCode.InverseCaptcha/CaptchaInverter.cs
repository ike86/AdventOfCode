using System;
using System.Linq;

namespace AdventOfCode.InverseCaptcha
{
    internal class CaptchaInverter
    {
        private string input;

        public CaptchaInverter(string input)
        {
            this.input = input;
        }

        internal int GetSum()
        {
            if (!input.Any())
            {
                return 0;
            }

            var sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == input[(i + 1) % input.Length])
                {
                    sum += int.Parse(input[i].ToString());
                }
            }

            return sum;
        }
    }
}