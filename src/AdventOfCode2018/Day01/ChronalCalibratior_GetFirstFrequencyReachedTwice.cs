using System.IO;
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
                    ToFrequencyChanges(1, -1));

                result.Should().Be(0);
            }

            [Theory, AutoData]
            public void Returns_freqency_by_repeating_the_input(ChronalCalibrator calibrator)
            {
                var result = calibrator.GetFirstFrequencyReachedTwice(
                    ToFrequencyChanges(3, 3, 4, -2, -4));

                result.Should().Be(10);
            }

            [Theory, AutoData]
            public void Returns_freqency_by_repeating_the_input_twice(ChronalCalibrator calibrator)
            {
                var result = calibrator.GetFirstFrequencyReachedTwice(
                    ToFrequencyChanges(-6, +3, +8, +5, -6));

                result.Should().Be(5);
            }

            [Theory, AutoData]
            public void Solves_day_1_problem_2(ChronalCalibrator calibrator)
            {
                var myPuzzleInput = File.ReadAllText("Day01/problem1.txt");
                calibrator.GetFirstFrequencyReachedTwice(myPuzzleInput).Should().Be(83130);
            }
        }
    }
}