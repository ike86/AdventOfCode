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

        public int GetFirstFrequencyReachedTwice(string frequencyChangesAsString)
        {
            var lines = Split(frequencyChangesAsString);
            var frequencyChanges = lines.Concat(lines).Concat(lines);
            var parsed = frequencyChanges.Select(ParseFrequencyChange);
            var reached = GetReachedFrequencies(parsed);
            return GetFirstReachedTwice(reached);
        }

        private static List<int> GetReachedFrequencies(IEnumerable<int> frequencyChanges)
            => frequencyChanges
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

        private static int GetFirstReachedTwice(IEnumerable<int> reached)
            => reached
                .GroupBy(i => i)
                .First(g => g.Count() > 1)
                .Key;

        private static string[] Split(string frequencyChanges)
            => frequencyChanges
                .Split(new[] { LineSeparator }, StringSplitOptions.RemoveEmptyEntries);

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
            return int.Parse(frequencyToken);
        }
    }
}