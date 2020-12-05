using System;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day05
    {
        [Theory]
        [InlineData(0, 0, 127)]
        [InlineData(1, 0, 63)]
        [InlineData(2, 32, 63)]
        [InlineData(3, 32, 47)]
        [InlineData(4, 40, 47)]
        [InlineData(5, 44, 47)]
        [InlineData(6, 44, 45)]
        [InlineData(7, 44, 44)]
        public void Calculate_row_range(int n, int start, int end)
        {
            var p = new BoardingPass("FBFBBFFRLR", n);

            p.RowRange.Should().Be(new Range(start, end));
        }
        
        [Fact]
        public void Calculate_row()
        {
            var p = new BoardingPass("FBFBBFFRLR");

            p.Row.Should().Be(44);
        }
    }

    public class BoardingPass
    {
        public BoardingPass(string raw, int n = 7)
        {
            var range = new Range(0,127);
            for (int i = 0; i < n; i++)
            {
                if (raw[i] == 'F')
                    range = LowerHalfOf(range);
                else if (raw[i] == 'B')
                    range = UpperHalfOf(range);
                else
                    throw new ArgumentOutOfRangeException();
            }
            
            RowRange = range;
        }

        public Range RowRange { get; }
        public int Row
        {
            get
            {
                if (RowRange.Start.Value != RowRange.End.Value)
                    throw new InvalidOperationException();

                return RowRange.Start.Value;
            }
        }

        private static Range UpperHalfOf(Range range)
        {
            return new Range(
                range.Start.Value + ((range.End.Value - range.Start.Value) / 2) + 1,
                range.End);
        }

        private static Range LowerHalfOf(Range range)
        {
            return new Range(
                range.Start,
                range.End.Value -((range.End.Value - range.Start.Value) / 2) - 1);
        }
    }
}