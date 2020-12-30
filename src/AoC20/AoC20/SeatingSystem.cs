using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day11
    {
        [Fact]
        public void Floor()
        {
            new WaitingArea(".")[0, 0].Should().Be(WaitingArea.Floor);
        }
    }

    public class WaitingArea
    {
        private readonly IPosition[][] _positions;

        public WaitingArea(string rawInitialLayout)
        {
            _positions = new[] {new[] {Floor}};
        }

        public static IPosition Floor { get; } = new Floor();

        public IPosition this[int i, int j] => _positions[i][j];
    }

    public interface IPosition
    {
    }

    public class Floor : IPosition
    {
    }
}