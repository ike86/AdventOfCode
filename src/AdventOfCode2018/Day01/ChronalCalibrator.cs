using System;
using System.Linq;

namespace AdventOfCode2018.Day01
{
    public class ChronalCalibrator
    {
        public const string LineSeparator = "\r\n";

        public int Calibrate(string frequencyChanges)
        {
            var lines = Split(frequencyChanges);

            return lines
                .Select(ParseFrequencyChange)
                .Sum();
        }

        private static string[] Split(string frequencyChanges)
        {
            return frequencyChanges
                .Split(new[] { LineSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static int ParseFrequencyChange(string line)
        {
            var sign = ParseSign(line);
            var frequencyChange = ParseChange(line);

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

        private static int ParseChange(string frequencyChanges)
        {
            var frequencyToken = frequencyChanges.Substring(1);

            var frequencyChange = int.Parse(frequencyToken);
            return frequencyChange;
        }
    }
}