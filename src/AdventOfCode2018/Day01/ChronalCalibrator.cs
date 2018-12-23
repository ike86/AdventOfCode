using System;
using System.Collections.Generic;
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

        public int GetFirstFrequencyReachedTwice(string frequencyChanges)
        {
            var lines = Split(frequencyChanges);

            var reachedFrequencies =
                lines
                .Select(ParseFrequencyChange)
                .Aggregate(
                    seed: (reachedFrequencies: new List<int> { 0 }, currentFrequency: 0),
                    (previous, current) =>
                    {
                        var (reached, currentFrequency) = previous;
                        currentFrequency += current;
                        reached.Add(currentFrequency);

                        return (reached, currentFrequency);
                    })
                .reachedFrequencies;

            return reachedFrequencies
                .GroupBy(i => i)
                .First(g => g.Count() > 1)
                .Key;
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