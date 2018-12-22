using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2018.Day01
{
    public class ChronalCalibratior_Calibrate
    {
        [Fact]
        public void Returns_zero_for_no_input()
        {
            var calibrator = new ChronalCalibrator();

            var result = calibrator.Calibrate(string.Empty);

            result.Should().Be(0);
        }
    }
}
