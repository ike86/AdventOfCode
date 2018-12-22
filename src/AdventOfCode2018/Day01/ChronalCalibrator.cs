using System;
using System.Linq;

namespace AdventOfCode2018.Day01
{
    public class ChronalCalibrator
    {
        public int Calibrate(string frequencyChanges)
        {
            if (frequencyChanges == string.Empty)
            {
                return 0;
            }

            var sign = ParseSign(frequencyChanges);
            var frequencyChange = ParseFrequencyChange(frequencyChanges);
            return sign * frequencyChange;
        }

        private static int ParseSign(string frequencyChange)
        {
            var signToken = frequencyChange.First();
            switch (signToken)
            {
                case '+':
                    return 1;

                case '-':
                    return -1;

                default:
                    throw new ArgumentException();
            }
        }

        private static int ParseFrequencyChange(string frequencyChanges)
        {
            var frequencyToken = frequencyChanges.Substring(1);

            var frequencyChange = int.Parse(frequencyToken);
            return frequencyChange;
        }
    }
}