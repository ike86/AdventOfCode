using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2018.Day01
{
    public partial class ChronalCalibratior_
    {
        public class GetFirstFrequencyReachedTwice
        {
            [Theory, AutoData]
            public void Returns_freqency_without_repeating_the_input(ChronalCalibrator calibrator)
            {
                var result = calibrator.GetFirstFrequencyReachedTwice(
                    ToFrequencyChanges(new[] { 1, -1 }));

                result.Should().Be(0);
            }

            [Theory, AutoData]
            public void Returns_freqency_by_repeating_the_input(ChronalCalibrator calibrator)
            {
                var result = calibrator.GetFirstFrequencyReachedTwice(
                    ToFrequencyChanges(new[] { 3, 3, 4, -2, -4 }));

                result.Should().Be(10);
            }
        }
    }
}