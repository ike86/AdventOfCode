using System;
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
    }

    public class WaitingArea
    {
        private readonly IPosition[][] _positions;

        public WaitingArea(string rawInitialLayout)
        {
            _positions = new[] {new[] {ParsePosition(rawInitialLayout)}};
        }

        private static IPosition ParsePosition(string rawPosition)
        {
            if (rawPosition == ".")
                return Floor;
            else if(rawPosition == "L")
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