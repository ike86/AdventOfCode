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

            var lines = frequencyChanges
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            return lines
                .Select(line =>
                {
                    var sign = ParseSign(line);
                    var frequencyChange = ParseFrequencyChange(line);

                    return sign * frequencyChange;
                })
                .Sum();
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