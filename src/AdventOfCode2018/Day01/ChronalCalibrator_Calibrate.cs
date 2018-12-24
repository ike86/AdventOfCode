using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using static AoC18.Day01.ChronalCalibrator;

namespace AoC18.Day01
{
    public partial class ChronalCalibrator_
    {
        public class Calibrate
        {
            [Theory, AutoData]
            public void Returns_zero_for_no_input(ChronalCalibrator calibrator)
            {
                var result = calibrator.Calibrate(string.Empty);

                result.Should().Be(0);
            }

            [Theory, AutoData]
            public void Returns_single_positive_frequency_change(
                ChronalCalibrator calibrator,
                int change)
            {
                var result = calibrator.Calibrate($"+{change}");

                result.Should().Be(change);
            }

            [Theory, AutoData]
            public void Returns_single_negative_frequency_change(
                ChronalCalibrator calibrator,
                int change)
            {
                var frequencyChange = -change;

                var result = calibrator.Calibrate($"{frequencyChange}");

                result.Should().Be(frequencyChange);
            }

            [Theory, AutoData]
            public void Returns_sum_of_many_frequency_changes(
                ChronalCalibrator calibrator,
                bool[] signs,
                int[] changes)
            {
                var frequencyChangesAsString = ToFrequencyChanges(signs, changes);
                var expected = signs
                    .Zip(changes, (s, n) => (s ? 1 : -1) * n)
                    .Sum();

                var result = calibrator.Calibrate(frequencyChangesAsString);

                result.Should().Be(expected);
            }

            [Theory, AutoData]
            public void Solves_day_1_problem_1(ChronalCalibrator calibrator)
            {
                var myPuzzleInput = File.ReadAllText("Day01/problem1.txt");
                calibrator.Calibrate(myPuzzleInput).Should().Be(587);
            }
        }

        private static string ToFrequencyChanges(bool[] signs, int[] numbers)
            => ToFrequencyChanges(
                signs
                .Zip(numbers, (sign, number) => sign ? number : -number));

        private static string ToFrequencyChanges(params int[] numbers)
            => ToFrequencyChanges(numbers.AsEnumerable());

        private static string ToFrequencyChanges(IEnumerable<int> numbers)
        {
            var frequencyChangeLines = numbers.Select(ToString);
            return string.Join(LineSeparator, frequencyChangeLines);
        }

        private static string ToString(int number)
            => $"{(number > 0 ? "+" : string.Empty)}{number}";
    }
}