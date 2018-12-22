using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2018.Day01
{
    public class ChronalCalibratior_Calibrate
    {
        [Theory, AutoData]
        public void Returns_zero_for_no_input(ChronalCalibrator calibrator)
        {
            var result = calibrator.Calibrate(string.Empty);

            result.Should().Be(0);
        }

        [Theory, AutoData]
        public void Returns_single_positive_number(ChronalCalibrator calibrator, int number)
        {
            var result = calibrator.Calibrate($"+{number}");

            result.Should().Be(number);
        }

        [Theory, AutoData]
        public void Returns_single_negative_number(ChronalCalibrator calibrator, int number)
        {
            number = -number;

            var result = calibrator.Calibrate($"{number}");

            result.Should().Be(number);
        }

        [Theory, AutoData]
        public void Returns_sum_of_many_frequency_changes(
            ChronalCalibrator calibrator,
            int[] numbers,
            bool[] signs)
        {
            var frequencyChangeLines = signs
                .Zip(numbers, (s, n) => $"{(s ? "+" : "-")}{n}");
            var frequencyChangesAsString = string.Join("\r\n", frequencyChangeLines);
            var expected = signs
                .Zip(numbers, (s, n) => (s ? 1 : -1) * n)
                .Sum();

            var result = calibrator.Calibrate(frequencyChangesAsString);

            result.Should().Be(expected);
        }
    }
}