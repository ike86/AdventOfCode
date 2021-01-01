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
        public void Parse_Floor() => new WaitingArea(".")[0, 0].Should().Be(WaitingArea.Floor);

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

        [Fact]
        public void Parse_waiting_area()
        {
            var a =
                ToWaitingArea(
                    @".L.
                      L.L
                      .L.");

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
            var waitingArea =
                ToWaitingArea(
                    @"L#
                      ##");
            var s = new Simulation(waitingArea);

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.EmptySeat);
        }

        [Fact]
        public void All_empty_becomes_all_occupied()
        {
            var s =
                new Simulation(
                    ToWaitingArea(
                        @"LL
                          LL"));

            var result = s.RunRounds(1);

            result[0, 0].Should().Be(WaitingArea.OccupiedSeat);
            result[0, 1].Should().Be(WaitingArea.OccupiedSeat);
            result[1, 0].Should().Be(WaitingArea.OccupiedSeat);
            result[1, 1].Should().Be(WaitingArea.OccupiedSeat);
        }

        [Fact]
        public void All_empty_3x3_becomes_all_occupied()
        {
            var s =
                new Simulation(
                    ToWaitingArea(
                        @"LLL
                          LLL
                          LLL"));

            var result = s.RunRounds(1);

            result[1, 0].Should().Be(WaitingArea.OccupiedSeat);
            result[0, 1].Should().Be(WaitingArea.OccupiedSeat);
            result[1, 2].Should().Be(WaitingArea.OccupiedSeat);
            result[2, 1].Should().Be(WaitingArea.OccupiedSeat);
            result[1, 1].Should().Be(WaitingArea.OccupiedSeat);
        }

        [Fact]
        public void Occupied_seat_with_less_than_4_occupied_adjacent_stays_occupied()
        {
            var s =
                new Simulation(
                    ToWaitingArea(
                        @"#L#
                          .#L
                          L#."));

            var result = s.RunRounds(1);

            result[1, 1].Should().Be(WaitingArea.OccupiedSeat);
        }

        [Fact]
        public void Occupied_seat_with_4_occupied_adjacent_becomes_empty()
        {
            var s =
                new Simulation(
                    ToWaitingArea(
                        @"#L#
                          ##L
                          L#."));

            var result = s.RunRounds(1);

            result[1, 1].Should().Be(WaitingArea.EmptySeat);
        }

        [Fact]
        public void Adjacent_works_as_expected()
        {
            var a =
                ToWaitingArea(
                    @".L#
                      #.L
                      L#.");

            a.AdjacentTo(0, 0).Should().HaveCount(2);
            a.AdjacentTo(1, 0).Should().HaveCount(4);
            a.AdjacentTo(1, 1).Should().HaveCount(6);
        }

        [Theory]
        [InlineData(1, "occupied")]
        [InlineData(2, "empty")]
        [InlineData(3, "empty")]
        [InlineData(100, "empty")]
        [InlineData(101, "empty")]
        public void Seat_alters_from_empty_to_occupied_to_empty_and_stays_empty(int n, string expected)
        {
            var s =
                new Simulation(
                    ToWaitingArea(
                        @"L.L
                          .L.
                          L.L"));

            var result = s.RunRounds(n);

            result[1, 1].Should().Be(
                expected switch
                {
                    "occupied" => WaitingArea.OccupiedSeat,
                    "empty" => WaitingArea.EmptySeat,
                    _ => throw new ArgumentOutOfRangeException()
                });
        }

        [Fact]
        public void Simulation_terminates_when_nothing_changes()
        {
            var s =
                new Simulation(
                    ToWaitingArea(
                        @"L.L
                          .L.
                          L.L"));

            Action act = () => s.Run();

            act.ExecutionTime().Should()
                .BeCloseTo(TimeSpan.FromMilliseconds(100), precision: TimeSpan.FromMilliseconds(100));
        }

        private const string InitialStateOfExample =
            @"L.LL.LL.LL
              LLLLLLL.LL
              L.L.L..L..
              LLLL.LL.LL
              L.LL.LL.LL
              L.LLLLL.LL
              ..L.L.....
              LLLLLLLLLL
              L.LLLLLL.L
              L.LLLLL.LL";

        private const string EndStateOfExample =
            @"#.#L.L#.##
              #LLL#LL.L#
              L.#.L..#..
              #L##.##.L#
              #.#L.LL.LL
              #.#L#L#.##
              ..L.L.....
              #L#L##L#L#
              #.LLLLLL.L
              #.#L#L#.##";

        [Fact]
        public void Example()
        {
            var s = new Simulation(ToWaitingArea(InitialStateOfExample));

            var result = s.Run();

            result.Should().BeEquivalentTo(ToWaitingArea(EndStateOfExample));
        }

        [Fact(Skip = "does not terminate")]
        public void Solve_puzzle()
        {
            var s = new Simulation(new WaitingArea(PuzzleInput.ForDay11));

            var result = s.Run();

            result.Positions.SelectMany(x => x).Count(p => p is OccupiedSeat)
                .Should().Be(2427);
        }

        [Fact]
        public void AdjacentTo_ignores_floor()
        {
            var a =
                ToWaitingArea(
                    @"..L..
                      .L.#.
                      L.#.L
                      .#.L.
                      ..#..");

            var adjacentSeats = a.AdjacentTo(2, 2).ToArray();

            adjacentSeats.Should().OnlyContain(p => !(p is Floor));
            adjacentSeats.Should().HaveCount(8);
        }
        
        [Fact]
        public void Part_2_example_1()
        {
            var a =
                ToWaitingArea(
                    @".......#.
                      ...#.....
                      .#.......
                      .........
                      ..#L....#
                      ....#....
                      .........
                      #........
                      ...#.....");

            var adjacentSeats = a.AdjacentTo(4, 3).ToArray();

            adjacentSeats.Should().HaveCount(8);
        }
        
        [Fact]
        public void Part_2_example_2()
        {
            var a =
                ToWaitingArea(
                    @".............
                      .L.L.#.#.#.#.
                      .............");

            var adjacentSeats = a.AdjacentTo(1, 1).ToArray();

            adjacentSeats.Should().HaveCount(1);
            adjacentSeats.Single().Should().BeOfType(typeof(EmptySeat));
        }
        
        [Fact]
        public void Part_2_example_3()
        {
            var a =
                ToWaitingArea(
                    @".##.##.
                      #.#.#.#
                      ##...##
                      ...L...
                      ##...##
                      #.#.#.#
                      .##.##.");

            var adjacentSeats = a.AdjacentTo(3, 3).ToArray();

            adjacentSeats.Should().BeEmpty();
        }
        
        private WaitingArea ToWaitingArea(string raw) => new(raw.Replace(" ", string.Empty));
    }

    public class Simulation
    {
        private WaitingArea _initialState;

        public Simulation(WaitingArea initialState)
        {
            _initialState = initialState;
        }

        public WaitingArea Run()
        {
            while (true)
            {
                var mutations = GetMutations().ToArray();
                if (mutations.IsEmpty())
                    return _initialState;
                
                _initialState = mutations.Aggregate(seed: _initialState, (a, m) => m.Execute(a));
            }
        }

        public WaitingArea RunRounds(int n)
        {
            for (int i = 0; i < n; i++)
            {
                var mutations = GetMutations().ToArray();
                _initialState = mutations.Aggregate(seed: _initialState, (a, m) => m.Execute(a));
            }

            return _initialState;
        }

        private IEnumerable<IMutation> GetMutations()
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

                if (position is OccupiedSeat)
                {
                    var adjacent = _initialState.AdjacentTo(i, j).ToArray();
                    if (adjacent.Count(p => p is OccupiedSeat) >= 4)
                        yield return new Empty(i, j);
                }
            }
        }
        
        private interface IMutation
        {
            WaitingArea Execute(WaitingArea a);
        }

        private class Occupy : IMutation
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
        private class Empty : IMutation
        {
            private readonly int _i;
            private readonly int _j;

            public Empty(int i, int j)
            {
                _i = i;
                _j = j;
            }

            public WaitingArea Execute(WaitingArea a)
            {
                a[_i, _j] = WaitingArea.EmptySeat;
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

        internal IEnumerable<IEnumerable<IPosition>> Positions => _positions;

        public IEnumerable<IPosition> AdjacentTo(int i, int j)
        {
            foreach (var (di, dj) in GetDirections())
            {
                for (
                    int k = i + di, l = j + dj;
                    IsInbounds(k, l);
                    k += di, l += dj)
                {
                    if (this[k, l] is not AoC20.Floor)
                    {
                        yield return this[k, l];
                        break;
                    }
                }
            }
            
            // var adjacentCoordinates =
            //     directions
            //     .Select(t => (i: i + t.di, j: j + t.dj));
            //
            // var adjacentPositions = adjacentCoordinates
            //     .Where(t => IsInbounds(t.i, t.j))
            //     .Select(t => this[t.i, t.j]);
            //
            // return adjacentPositions;
        }

        private bool IsInbounds(int i, int j)
        {
            return i >= 0
                   && j >= 0
                   && i <= _positions.Length - 1
                   && j <= _positions[i].Length - 1;
        }

        private static IEnumerable<(int di, int dj)> GetDirections()
        {
            return new[] {-1, 0, 1}.Join(new[] {-1, 0, 1}, _ => 0, _ => 0, (di, dj) => (di, dj))
                .Where(t => t != (0, 0));
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

    public static partial class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();
    }
}