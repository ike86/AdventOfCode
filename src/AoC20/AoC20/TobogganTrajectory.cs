using System;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day03
    {
        private const string Example =
@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
        
        [Fact]
        public void Map_CountTreesOnSlope_can_count_downwards()
        {
            var map = new Map(Example);

            map.CountTreesOnSlope(right: 0, down: 1)
                .Should().Be(3);
        }
    }

    public class Map
    {
        private readonly string[] _lines;

        public Map(string raw)
        {
            _lines = raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public int CountTreesOnSlope(int right, int down)
        {
            var count = 0;
            for (int i = 0; i < _lines.Length; i++)
            {
                if (_lines[i][0] == '#')
                {
                    count += 1;
                }
            }
            return count;
        }
    }

    public class TobogganTrajectory
    {
        
    }
}