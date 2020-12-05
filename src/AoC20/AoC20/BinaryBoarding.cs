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
        
        // FBFBBFFRLR
            
        [Fact]
        public void Calculate_row()
        {
            var p = new BoardingPass("FB");

            p.Row.Should().Be(new Range(32, 63));
        }
    }

    public class BoardingPass
    {
        public BoardingPass(string raw)
        {
            var range = new Range(0,127);
            if(raw.First() == 'F')
                range= new Range(range.Start, range.End.Value / 2);
            else if (raw.First() == 'B')
                range = new Range(range.End.Value / 2 + 1, range.End);
            else
                throw new ArgumentOutOfRangeException();
            
            if(raw.Skip(1).First() == 'F')
                range= new Range(range.Start, range.End.Value / 2);
            else if (raw.Skip(1).First() == 'B')
                range = new Range(range.End.Value / 2 + 1, range.End);
            else
                throw new ArgumentOutOfRangeException();
            
            Row = range;
        }

        public Range Row { get; set; }
    }
}