using System;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2017.Day01
{
    public class Test_CaptchaInverter2_GetSum
    {
        [Fact]
        public void ThrowsError_If_InputHasOddNumberOfDigits()
        {
            Action act = () =>
                new CaptchaInverter2("123");

            act.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ReturnsZero_If_InputOnlyHasTwoDifferentDigits()
        {
            var inverter = new CaptchaInverter2("12");

            var sum = inverter.GetSum();

            sum.Should().Be(0);
        }

        [Fact]
        public void ReturnsTheDigit_If_InputOnlyHasTwoSameDigits()
        {
            var inverter = new CaptchaInverter2("22");

            var sum = inverter.GetSum();

            sum.Should().Be(4);
        }

        [Theory]
        [InlineData("1212", 6)]
        [InlineData("1221", 0)]
        [InlineData("123425", 4)]
        [InlineData("123123", 12)]
        [InlineData("12131415", 4)]
        public void ReturnsExpected(string input, int expected)
        {
            var inverter = new CaptchaInverter2(input);

            var sum = inverter.GetSum();

            sum.Should().Be(expected);
        }
    }
}