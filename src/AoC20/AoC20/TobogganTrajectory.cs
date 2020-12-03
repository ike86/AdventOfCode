﻿using System;
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
        
        [Fact]
        public void Map_can_be_indexed()
        {
            var map = new Map(Example);

            map[0, 0].Should().Be(Map.Open);
            map[0, 2].Should().Be(Map.Tree);
            map[2, 0].Should().Be(Map.Open);
        }
    }

    public class Map
    {
        public static Square Open = new Open();
        public static Square Tree = new Tree();
        
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

        public Square this[int i, int j]
        {
            get
            {
                if (_lines[i][j] == '.')
                {
                    return new Open();
                }
                else if (_lines[i][j] == '#')
                {
                    return new Tree();
                }

                throw new InvalidOperationException();
            }
        }
    }

    public interface Square
    {
    }

    public class Tree : Square, IEquatable<Tree>
    {
        public bool Equals(Tree other) => true;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tree) obj);
        }

        public override int GetHashCode() => typeof(Tree).GetHashCode();

        public static bool operator ==(Tree left, Tree right) => Equals(left, right);

        public static bool operator !=(Tree left, Tree right) => !Equals(left, right);
    }

    public class Open : Square, IEquatable<Open>
    {
        public bool Equals(Open other) => true;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Open) obj);
        }

        public override int GetHashCode() => typeof(Open).GetHashCode();

        public static bool operator ==(Open left, Open right) => Equals(left, right);

        public static bool operator !=(Open left, Open right) => !Equals(left, right);
    }
}