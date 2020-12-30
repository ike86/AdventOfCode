using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day11
    {
        [Fact]
        public void Floor() => new WaitingArea(".")[0, 0].Should().Be(WaitingArea.Floor);
        
        [Fact]
        public void Empty_seat() => new WaitingArea("L")[0, 0].Should().Be(WaitingArea.EmptySeat);
        
        [Fact]
        public void Parse_row()
        {
            var a = new WaitingArea(".L.");
            
            a[0, 0].Should().Be(WaitingArea.Floor);
            a[0, 1].Should().Be(WaitingArea.EmptySeat);
            a[0, 2].Should().Be(WaitingArea.Floor);
        }
    }

    public class WaitingArea
    {
        private readonly IPosition[][] _positions;

        public WaitingArea(string rawInitialLayout)
        {
            _positions = new[] {ParseRowOfPositions(rawInitialLayout)};
        }

        private static IPosition[] ParseRowOfPositions(string rawInitialLayout)
        {
            return rawInitialLayout
                .Select(ch => ParsePosition(ch))
                .ToArray();
        }

        private static IPosition ParsePosition(char rawPosition)
        {
            if (rawPosition == '.')
                return Floor;
            else if(rawPosition == 'L')
                return EmptySeat;
            else
                throw new ArgumentOutOfRangeException();
        }

        public static IPosition Floor { get; } = new Floor();
        
        public static IPosition EmptySeat { get; } = new EmptySeat();

        public IPosition this[int i, int j] => _positions[i][j];
    }

    public interface IPosition
    {
    }

    public class Floor : IPosition
    {
    }

    public class EmptySeat : IPosition
    {
    }
}