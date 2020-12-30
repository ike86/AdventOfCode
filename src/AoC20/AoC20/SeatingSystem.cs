using System;
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
            if (_initialState[0, 0] is EmptySeat)
            {
                var adjacent = _initialState.AdjacentTo(0, 0);
                if(adjacent.Any(p => p is OccupiedSeat))
                    return _initialState;
                
                _initialState[0, 0] = WaitingArea.OccupiedSeat;
            }

            return _initialState;
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
            if(_positions.Length -1 >= 1)
                yield return this[1,0];
            
            if(_positions[0].Length -1 >= 1)
                yield return this[0,1];
            
            if (_positions.Length - 1 >= 1
                && _positions[1].Length - 1 >= 1)
                yield return this[1, 1];
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