using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using static AdventOfCode2018.Day01.ChronalCalibrator;

namespace AdventOfCode2018.Day01
{
    public class ChronalCalibratior_GetFirstFrequencyReachedTwice
    {
        [Theory, AutoData]
        public void Returns_freqency_without_repeating_the_input(ChronalCalibrator calibrator)
        {
            var result = calibrator.GetFirstFrequencyReachedTwice(
                ToFrequencyChanges(new[] { true, false }, new[] { 1, 1 }));

            result.Should().Be(0);
        }

        private static string ToFrequencyChanges(bool[] signs, int[] numbers)
        {
            var frequencyChangeLines = signs.Zip(numbers, (s, n) => $"{(s ? "+" : "-")}{n}");
            return string.Join(LineSeparator, frequencyChangeLines);
        }
    }
}
