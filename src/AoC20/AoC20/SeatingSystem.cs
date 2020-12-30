﻿using System;
using System.Collections.Generic;
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
        public void Occupied_seat() => new WaitingArea("#")[0, 0].Should().Be(WaitingArea.OccupiedSeat);
        
        [Fact]
        public void Parse_row()
        {
            var a = new WaitingArea(".L.");
            
            a[0, 0].Should().Be(WaitingArea.Floor);
            a[0, 1].Should().Be(WaitingArea.EmptySeat);
            a[0, 2].Should().Be(WaitingArea.Floor);
        }
        private const string ParseExample =
@".L.
L.L
.L.";
        
        [Fact]
        public void Parse_waiting_area()
        {
            var a = new WaitingArea(ParseExample);
            
            a[0, 0].Should().Be(WaitingArea.Floor);
            a[0, 1].Should().Be(WaitingArea.EmptySeat);
            a[0, 2].Should().Be(WaitingArea.Floor);
            
            a[1, 0].Should().Be(WaitingArea.EmptySeat);
            a[1, 1].Should().Be(WaitingArea.Floor);
            a[1, 2].Should().Be(WaitingArea.EmptySeat);
            
            a[2, 0].Should().Be(WaitingArea.Floor);
            a[2, 1].Should().Be(WaitingArea.EmptySeat);
            a[2, 2].Should().Be(WaitingArea.Floor);
        }

        [Fact]
        public void Lone_empty_seat_becomes_occupied()
        {
            var s = new Simulation(new WaitingArea("L"));

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.OccupiedSeat);
        }
        
        [Fact]
        public void Floor_stays_floor()
        {
            var s = new Simulation(new WaitingArea("."));

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.Floor);
        }
        
        [Fact]
        public void Lone_occupied_seat_stays_occupied()
        {
            var s = new Simulation(new WaitingArea("#"));

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.OccupiedSeat);
        }
        
        [Fact]
        public void Top_left_empty_seat_stays_empty_if_any_adjacent_is_occupied()
        {
            var s =
                new Simulation(
                    new WaitingArea(
                        "L#" + Environment.NewLine
                      + "##"));

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.EmptySeat);
        }
        
        [Fact]
        public void All_empty_becomes_all_occupied()
        {
            var s =
                new Simulation(
                    new WaitingArea(
                        "LL" + Environment.NewLine
                      + "LL"));

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.OccupiedSeat);
            result[0, 1].Should().Be(WaitingArea.OccupiedSeat);
            result[1, 0].Should().Be(WaitingArea.OccupiedSeat);
            result[1, 1].Should().Be(WaitingArea.OccupiedSeat);
        }
    }

    public class Simulation
    {
        private readonly WaitingArea _initialState;

        public Simulation(WaitingArea initialState)
        {
            _initialState = initialState;
        }

        public WaitingArea RunRounds(int n)
        {
            var mutations = GetMutations().ToArray();

            return mutations.Aggregate(seed: _initialState, (a, m) => m.Execute(a));
        }

        private IEnumerable<Occupy> GetMutations()
        {
            foreach (var (i, j, position) in _initialState.AllIndexedPositions())
            {
                if (position is EmptySeat)
                {
                    var adjacent = _initialState.AdjacentTo(i, j);
                    if (adjacent.Any(p => p is OccupiedSeat))
                        continue;

                    yield return new Occupy(i, j);
                }
            }
        }

        private class Occupy
        {
            private readonly int _i;
            private readonly int _j;

            public Occupy(int i, int j)
            {
                _i = i;
                _j = j;
            }

            public WaitingArea Execute(WaitingArea a)
            {
                a[_i, _j] = WaitingArea.OccupiedSeat;
                return a;
            }
        }  
    }

    public class WaitingArea
    {
        private readonly IPosition[][] _positions;

        public WaitingArea(string rawInitialLayout)
        {
            _positions = WaitingAreaParser.Parse(rawInitialLayout);
        }

        public static IPosition Floor { get; } = new Floor();
        
        public static IPosition EmptySeat { get; } = new EmptySeat();
        
        public static IPosition OccupiedSeat { get; } = new OccupiedSeat();

        public IPosition this[int i, int j]
        {
            get => _positions[i][j];
            set => _positions[i][j] = value;
        }

        public IEnumerable<IPosition> AdjacentTo(int i, int j)
        {
            return
                new[] {-1, 0, 1}.Join(new[] {-1, 0, 1}, _ => 0, _ => 0, (di, dj) => (di, dj))
                    .Where(t => t.di != 0 && t.dj != 0)
                    .Select(t => (i: i + t.di, j: j + t.dj))
                    .Where(t =>
                        t.i >= 0
                        && t.j >= 0
                        && t.i <= _positions.Length - 1
                        && t.j <= _positions[i].Length - 1)
                    .Select(t => this[t.i, t.j]);
        }

        public IEnumerable<(int i, int j, IPosition position)> AllIndexedPositions()
        {
            for (int i = 0; i < _positions.Length; i++)
            {
                for (int j = 0; j < _positions[i].Length; j++)
                {
                    yield return (i, j, this[i, j]);
                }
            }
        }
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

    public class OccupiedSeat : IPosition
    {
    }

    public class WaitingAreaParser
    {
        public static IPosition[][] Parse(string rawInitialLayout)
        {
            return rawInitialLayout.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(rawRow => ParseRowOfPositions(rawRow))
                .ToArray();
        }

        private static IPosition[] ParseRowOfPositions(string rawRow)
        {
            return rawRow
                .Select(ch => ParsePosition(ch))
                .ToArray();
        }

        private static IPosition ParsePosition(char rawPosition)
        {
            if (rawPosition == '.')
                return WaitingArea.Floor;
            else if(rawPosition == 'L')
                return WaitingArea.EmptySeat;
            else if (rawPosition == '#')
                return WaitingArea.OccupiedSeat;
            else
                throw new ArgumentOutOfRangeException();
        }
    }
}