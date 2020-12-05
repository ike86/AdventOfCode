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
        
        [Fact]
        public void Step_one_towards_the_back()
        {
            var p = new BoardingPass("B");

            p.Row.Should().Be(new Range(64, 127));
        }
    }

    public class BoardingPass
    {
        public BoardingPass(string raw)
        {
            var start = 0;
            var end = 127;
            Range range;
            if(raw.First() == 'F')
                range= new Range(start, end / 2);
            else if (raw.First() == 'B')
                range = new Range(end / 2 + 1, end);
            else
                throw new ArgumentOutOfRangeException();
            Row = range;
        }

        public Range Row { get; set; }
    }
}