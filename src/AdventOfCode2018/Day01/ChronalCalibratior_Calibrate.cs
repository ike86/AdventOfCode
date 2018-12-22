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
    }
}