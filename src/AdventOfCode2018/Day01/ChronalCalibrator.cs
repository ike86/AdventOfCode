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
            var frequencyChanges = Split(frequencyChangesAsString);
            var parsed = frequencyChanges.Select(ParseFrequencyChange);
            using (var infiniteLoopingEnumerator = new InfiniteLoopingEnumerator<int>(parsed))
            {
                var looping = infiniteLoopingEnumerator.AsEnumerable();
                var reached = GetReachedFrequencies(looping);
                return GetFirstReachedTwice(reached);
            }
        }

        private static IEnumerable<int> GetReachedFrequencies(IEnumerable<int> frequencyChanges)
        {
            var currentFrequency = 0;
            yield return currentFrequency;
            foreach (var frequencyChange in frequencyChanges)
            {
                currentFrequency += frequencyChange;
                yield return currentFrequency;
            }
        }

        private static int GetFirstReachedTwice(IEnumerable<int> reached)
        {
            var singleFrequencies = new HashSet<int>();
            foreach (var frequency in reached)
            {
                if (singleFrequencies.Contains(frequency))
                {
                    return frequency;
                }

                singleFrequencies.Add(frequency);
            }

            throw new ArgumentException();
        }

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