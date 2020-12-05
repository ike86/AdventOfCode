using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day05
    {
        [Fact]
        public void Step_one_towards_the_front()
        {
            var p = new BoardingPass("F");

            p.Row.Should().Be(new Range(0, 63));
        }
    }

    public class BoardingPass
    {
        public BoardingPass(string raw)
        {
            var start = 0;
            var end = 127;
            Row = new Range(start, end / 2);
        }

        public Range Row { get; set; }
    }
}