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
        
        [Theory]
        [InlineData(7, 0, 7)]
        [InlineData(8, 4, 7)]
        [InlineData(9, 4, 5)]
        [InlineData(10, 5, 5)]
        public void Calculate_row_and_column_range(int n, int start, int end)
        {
            var p = new BoardingPass("FBFBBFFRLR", n);

            p.ColumnRange.Should().Be(new Range(start,end));
        }
        
        [Fact]
        public void Calculate_column()
        {
            var p = new BoardingPass("FBFBBFFRLR");

            p.Column.Should().Be(5);
        }
    }

    public class BoardingPass
    {
        private const int ExpectedLength = 10;
        private const int LengthOfRowCode = 7;

        public BoardingPass(string raw, int n = ExpectedLength)
        {
            {
                var range = new Range(0, 127);
                for (int i = 0; i < Math.Min(n, LengthOfRowCode); i++)
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

            {
                var range = new Range(0, LengthOfRowCode);
                for (int i = LengthOfRowCode; i < Math.Min(n, ExpectedLength); i++)
                {
                    if (raw[i] == 'L')
                        range = LowerHalfOf(range);
                    else if (raw[i] == 'R')
                        range = UpperHalfOf(range);
                    else
                        throw new ArgumentOutOfRangeException();
                }

                ColumnRange = range;
            }
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

        public Range ColumnRange { get; }
        
        public int Column
        {
            get
            {
                if (ColumnRange.Start.Value != ColumnRange.End.Value)
                    throw new InvalidOperationException();

                return ColumnRange.Start.Value;
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