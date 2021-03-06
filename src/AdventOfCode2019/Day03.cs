﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC19
{
    public class Day03
    {
        /*
        --- Day 3: Crossed Wires ---
        The gravity assist was successful, and you're well on your way to the Venus refuelling station.
        During the rush back on Earth, the fuel management system wasn't completely installed,
        so that's next on the priority list.

        Opening the front panel reveals a jumble of wires.
        Specifically, two wires are connected to a central port and extend outward on a grid.
        You trace the path each wire takes as it leaves the central port, one wire per line of text (your puzzle input).
        */

        [Theory, AutoData]
        void Interpreter_has_a_grid_with_one_wire_in_origo_by_default(WirePathInterpreter i)
        {
            i.Grid[0, 0].Should().Be(1);
        }

        [Theory, AutoData]
        void Interpret_a_path_segment_to_left(WirePathInterpreter i, int length)
        {
            i.Interpret((Direction.Left, length));

            Range(length, x => i.Grid[-x, 0]).Should().AllBeEquivalentTo(1);
        }

        [Theory, AutoData]
        void Interpret_a_path_segment_to_right(WirePathInterpreter i, int length)
        {
            i.Interpret((Direction.Right, length));

            Range(length, x => i.Grid[x, 0]).Should().AllBeEquivalentTo(1);
        }

        [Theory, AutoData]
        void Interpret_a_path_segment_to_up(WirePathInterpreter i, int length)
        {
            i.Interpret((Direction.Up, length));

            Range(length, x => i.Grid[0, x]).Should().AllBeEquivalentTo(1);
        }

        [Theory, AutoData]
        void Interpret_a_path_segment_to_down(WirePathInterpreter i, int length)
        {
            i.Interpret((Direction.Down, length));

            Range(length, x => i.Grid[0, -x]).Should().AllBeEquivalentTo(1);
        }

        private static IEnumerable<int> Range(int length, Func<int, int> selector)
        {
            return Enumerable.Range(1, length).Select(selector);
        }

        [Theory, AutoData]
        void Grid_returns_zero_where_no_cable_is(Grid grid, int x, int y)
        {
            grid[x, y].Should().Be(0);
        }

        [Theory, AutoData]
        void Interpret_two_path_segments_in_L_shape(WirePathInterpreter i)
        {
            var lengthDown = 5;
            var lengthRight = 3;
            i.Interpret((Direction.Down, lengthDown), (Direction.Right, lengthRight));

            using var a = new AssertionScope();
            Range(lengthDown, x => i.Grid[0, -x]).Should().AllBeEquivalentTo(1);
            Range(lengthRight, x => i.Grid[x, -lengthDown]).Should().AllBeEquivalentTo(1);
            i.Grid.AsString().Should().Be(
                "......" + NL +
                ".1...." + NL +
                ".1...." + NL +
                ".1...." + NL +
                ".1...." + NL +
                ".1...." + NL +
                ".1111." + NL +
                "......" + NL);
        }

        [Theory, AutoData]
        void Interpret_two_path_segments_in_r_shape(WirePathInterpreter i)
        {
            var lengthDown = 5;
            var lengthRight = 3;
            i.Interpret((Direction.Right, lengthRight), (Direction.Down, lengthDown));

            using var a = new AssertionScope();
            Range(lengthRight, x => i.Grid[x, 0]).Should().AllBeEquivalentTo(1);
            Range(lengthDown, y => i.Grid[lengthRight, -y]).Should().AllBeEquivalentTo(1);
            i.Grid.AsString().Should().Be(
                "......" + NL +
                ".1111." + NL +
                "....1." + NL +
                "....1." + NL +
                "....1." + NL +
                "....1." + NL +
                "....1." + NL +
                "......" + NL);
        }

        [Theory, AutoData]
        void Interpret_three_path_segments(WirePathInterpreter i)
        {
            var lengthDown = 5;
            var lengthRight = 3;
            var lengthUp = 2;
            i.Interpret((Direction.Down, lengthDown), (Direction.Right, lengthRight), (Direction.Up, lengthUp));

            using var a = new AssertionScope();
            Range(lengthDown, x => i.Grid[0, -x]).Should().AllBeEquivalentTo(1);
            Range(lengthRight, x => i.Grid[x, -lengthDown]).Should().AllBeEquivalentTo(1);
            i.Grid.AsString().Should().Be(
                "......" + NL +
                ".1...." + NL +
                ".1...." + NL +
                ".1...." + NL +
                ".1..1." + NL +
                ".1..1." + NL +
                ".1111." + NL +
                "......" + NL);
        }

        [Theory, AutoData]
        void Interpret_self_crossing_path_segments(WirePathInterpreter i)
        {
            i.Interpret(
                (Direction.Down, 5),
                (Direction.Right, 3),
                (Direction.Up, 2),
                (Direction.Left, 4));

            i.Grid.AsString().Should().Be(
                "......." + NL +
                "..1...." + NL +
                "..1...." + NL +
                "..1...." + NL +
                ".11111." + NL +
                "..1..1." + NL +
                "..1111." + NL +
                "......." + NL);
        }

        [Theory, AutoData]
        void Interpret_two_wire_paths(WirePathInterpreter i)
        {
            var lengthDown = 5;
            var lengthRight = 3;
            i.Interpret(wireId: 1, (Direction.Down, lengthDown), (Direction.Right, lengthRight));
            i.Interpret(wireId: 2, (Direction.Right, lengthRight - 1), (Direction.Down, lengthDown));

            using var a = new AssertionScope();
            i.Grid.AsString().Should().Be(
                "......" + NL +
                ".111.." + NL +
                ".1.1.." + NL +
                ".1.1.." + NL +
                ".1.1.." + NL +
                ".1.1.." + NL +
                ".1121." + NL +
                "......" + NL);
        }

        static readonly string NL = Environment.NewLine;

        [Theory, AutoData]
        void Grid_AsString_visualizes_grid(Grid g)
        {
            AddWire(0, 1);
            AddWire(1, 0);
            AddWire(0, -1);
            AddWire(-1, 0);

            g.AsString().Should().Be(
                "....." + NL +
                "..1.." + NL +
                ".111." + NL +
                "..1.." + NL +
                "....." + NL);

            void AddWire(int x, int y)
            {
                g.AddWire(x, y, atLayer: 0, wireLengthFromOrigo: 0);
            }
        }

        class WirePathInterpreter
        {
            public Grid Grid { get; internal set; } = new Grid();

            /// <summary>
            /// Only exposed to support unit tests.
            /// </summary>
            internal void Interpret(params (Direction direction, int length)[] pathSegments) =>
                Interpret(wireId: 0, pathSegments);

            internal void Interpret(int wireId, params (Direction direction, int length)[] pathSegments)
            {
                var x = 0;
                var y = 0;
                var wireLengthFromOrigo = 0;
                foreach (var (direction, length) in pathSegments)
                {
                    for (int i = 1; i <= length; i++)
                    {
                        switch (direction)
                        {
                            case Direction.Left:
                                x -= 1;
                                break;
                            case Direction.Right:
                                x += 1;
                                break;
                            case Direction.Up:
                                y += 1;
                                break;
                            case Direction.Down:
                                y -= 1;
                                break;
                        }

                        wireLengthFromOrigo += 1;
                        Grid.AddWire(x, y, atLayer: wireId, wireLengthFromOrigo);
                    }
                }
            }
        }

        class Grid
        {
            private class GridLayer : Dictionary<(int x, int y), WireSegment> { }

            private readonly Dictionary<int, GridLayer> gridLayers =
                new Dictionary<int, GridLayer>();

            public Grid()
            {
                var layer = new GridLayer();
                layer.Add((0, 0), new WireSegment(0));
                gridLayers.Add(0, layer);
            }

            public int this[int x, int y] => GetWireCount((x, y));

            public IEnumerable<(int x, int y, int numberOfWires)> WirePositions =>
                Positions
                .Select(p => (position: p, wireCount: GetWireCount(p)))
                .Where(t => t.wireCount > 0)
                .Select(t => (t.position.x, t.position.y, t.wireCount));

            public IEnumerable<(int x, int y, int wireLengthFromOrigo)> CombinedWireLengths =>
                Positions
                .Select(p =>
                    (position: p,
                    wireLengthFromOrigo: gridLayers.Keys.Sum(i => GetWireLengthFromOrigo(p, i))))
                .Where(t => t.wireLengthFromOrigo > 0)
                .Select(t => (t.position.x, t.position.y, t.wireLengthFromOrigo));

            public IEnumerable<(int x, int y)> Intersections =>
                WirePositions
                .Where(t => t.numberOfWires == 2)
                .Select(t => (t.x, t.y));

            private IEnumerable<(int x, int y)> Positions
                => gridLayers.Values.SelectMany(x => x.Keys).Distinct();

            internal void AddWire(int x, int y, int atLayer, int wireLengthFromOrigo)
            {
                if (!gridLayers.ContainsKey(atLayer))
                {
                    gridLayers.Add(atLayer, new GridLayer());
                }

                if (gridLayers[atLayer].TryGetValue((x, y), out var w)
                    && w is WireSegment wire)
                {
                    gridLayers[atLayer][(x, y)] =
                        new WireSegment(wire.MinimumLengthFromOrigo);
                }
                else
                {
                    gridLayers[atLayer][(x, y)] = new WireSegment(wireLengthFromOrigo);
                }
            }

            internal string AsString()
            {
                int maxX = Positions.Max(p => p.x);
                int minX = Positions.Min(p => p.x);
                int maxY = Positions.Max(p => p.y);
                int minY = Positions.Min(p => p.y);
                var result = "";
                for (int y = maxY + 1; y >= minY - 1; y--)
                {
                    for (int x = minX - 1; x <= maxX + 1; x++)
                    {
                        result += this[x, y] > 0 ? this[x, y].ToString() : ".";
                    }
                    result += Environment.NewLine;
                }
                return result;
            }

            internal string AsStringWithWireLengths(int atLayer = 0)
            {
                int maxX = Positions.Max(p => p.x);
                int minX = Positions.Min(p => p.x);
                int maxY = Positions.Max(p => p.y);
                int minY = Positions.Min(p => p.y);
                var result = "";
                for (int y = maxY + 1; y >= minY - 1; y--)
                {
                    for (int x = minX - 1; x <= maxX + 1; x++)
                    {
                        var wireLength = GetWireLengthFromOrigo((x, y), atLayer);
                        result += wireLength > 0 ? wireLength.ToString() : ".";
                    }
                    result += Environment.NewLine;
                }
                return result;
            }

            private int GetWireCount((int x, int y) p)
            {
                return
                    gridLayers
                    .Select(kvp =>
                    {
                        kvp.Value.TryGetValue(p, out WireSegment wire);
                        return wire is WireSegment ? 1 : 0;
                    })
                    .Sum();
            }

            private int GetWireLengthFromOrigo((int x, int y) p, int atLayer)
            {
                gridLayers[atLayer].TryGetValue(p, out var wire);
                return wire?.MinimumLengthFromOrigo ?? 0;
            }
        }

        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        private class WireSegment
        {
            public WireSegment(int minimumLengthFromOrigo)
            {
                MinimumLengthFromOrigo = minimumLengthFromOrigo;
            }

            public int MinimumLengthFromOrigo { get; }
        }

        [Fact]
        void Parsing_empty_wire_path_yields_no_path_segments()
        {
            ParseWirePath("").Should().BeEmpty();
        }

        [Fact]
        void Parsing_wire_path_with_one_path_segment()
        {
            using var a = new AssertionScope();
            ParseWirePath("R8").Should().Contain((direction: Direction.Right, length: 8));
            ParseWirePath("R8").Should().ContainSingle();
        }

        [Fact]
        void Parsing_wire_path_with_multiple_path_segments()
        {
            ParseWirePath("R8,U5,L1,D33").Should().BeEquivalentTo(
                (direction: Direction.Right, length: 8),
                (direction: Direction.Up, length: 5),
                (direction: Direction.Left, length: 1),
                (direction: Direction.Down, length: 33));
        }

        private IEnumerable<(Direction direction, int length)> ParseWirePath(string path)
        {
            var pathSegmentTokens =
                path.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in pathSegmentTokens)
            {
                var direction =
                    token[0] switch
                    {
                        'R' => Direction.Right,
                        'U' => Direction.Up,
                        'L' => Direction.Left,
                        'D' => Direction.Down,
                    };
                var length = int.Parse(token.Substring(1));
                yield return (direction, length);
            }
        }

        /*

        The wires twist and turn, but the two wires occasionally cross paths.
        To fix the circuit, you need to find the intersection point closest to the central port.
        Because the wires are on a grid, use the Manhattan distance for this measurement.
        While the wires do technically cross right at the central port where they both start,
        this point does not count, nor does a wire count as crossing with itself.
        */

        [Theory, AutoData]
        void Manhattan_distance_of_identical_points_is_zero(int x, int y)
        {
            ManhattanDistanceOf((x, y), (x, y)).Should().Be(0);
        }

        [Theory]
        [InlineAutoData(1, 1)]
        [InlineAutoData(-1, 1)]
        [InlineAutoData(1, -1)]
        [InlineAutoData(-1, -1)]
        void Manhattan_distance_of_distinct_points(int sdx, int sdy, int x, int y, int dx, int dy)
        {
            int distance = ManhattanDistanceOf((x, y), (x + (sdx * dx), y + (sdy * dy)));
            distance.Should().Be(dx + dy);
        }

        private int ManhattanDistanceOf((int x, int y) p1, (int x, int y) p2)
        {
            return Math.Abs(p2.x - p1.x) + Math.Abs(p2.y - p1.y);
        }

        /*
        For example, if the first wire's path is R8,U5,L5,D3,
        then starting from the central port (o), it goes right 8, up 5, left 5, and finally down 3:

        ...........
        ...........
        ...........
        ....+----+.
        ....|....|.
        ....|....|.
        ....|....|.
        .........|.
        .o-------+.
        ...........
        Then, if the second wire's path is U7,R6,D4,L4, it goes up 7, right 6, down 4, and left 4:

        ...........
        .+-----+...
        .|.....|...
        .|..+--X-+.
        .|..|..|.|.
        .|.-X--+.|.
        .|..|....|.
        .|.......|.
        .o-------+.
        ...........
        These wires cross at two locations (marked X),
        but the lower-left one is closer to the central port: its distance is 3 + 3 = 6.

        Here are a few more examples:

        R75,D30,R83,U83,L12,D49,R71,U7,L72
        U62,R66,U55,R34,D71,R55,D58,R83 = distance 159
        R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
        U98,R91,D20,R16,D67,R40,U7,R15,U6,R7 = distance 135
        What is the Manhattan distance from the central port to the closest intersection?
        */

        [Theory]
        [InlineData("R8,U5,L5,D3\r\nU7,R6,D4,L4", 6)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72\r\nU62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\r\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        [InlineData(MyPuzzleInput, 2050)]
        void Closest_wire_intersection_to_the_center_is_as_expected(
            string paths,
            int expectedDistance)
        {
            int distance = DistanceOfClosestIntersectionToCenter(paths);

            distance.Should().Be(expectedDistance);
        }

        private int DistanceOfClosestIntersectionToCenter(string paths)
        {
            var i = new WirePathInterpreter();
            var wireId = 1;
            foreach (var pathSegments in ParseWirePaths(paths))
            {
                i.Interpret(wireId, pathSegments.ToArray());
                wireId += 1;
            }

            return
                i.Grid.WirePositions
                .Where(t => t.numberOfWires == 2)
                .Min(t => ManhattanDistanceOf((0, 0), (t.x, t.y)));
        }

        private IEnumerable<IEnumerable<(Direction direction, int length)>> ParseWirePaths(
            string paths)
            => paths
            .Split(new[] { NL }, StringSplitOptions.RemoveEmptyEntries)
            .Select(ParseWirePath);

        private const string MyPuzzleInput =
            @"R1003,U476,L876,U147,R127,D10,R857,U199,R986,U311,R536,D930,R276,U589,L515,D163,L660,U69,R181,D596,L37,D359,R69,D50,L876,D867,L958,U201,R91,D127,R385,U646,L779,D309,L577,U535,R665,D669,L640,D50,L841,D32,R278,U302,L529,U679,R225,U697,R94,D205,L749,U110,L132,U664,R122,U476,R596,U399,R145,U995,R821,U80,L853,U461,L775,U57,R726,U299,L706,U500,R520,U608,L349,D636,L352,U617,R790,U947,L377,D995,R37,U445,L706,D133,R519,D194,L473,U330,L788,D599,L466,D100,L23,D68,R412,U566,R43,U333,L159,D18,L671,U135,R682,D222,R651,U138,R904,U546,R871,U264,R133,U19,R413,D235,R830,D376,R530,U18,L476,D120,L190,D252,R105,D874,L544,D705,R351,U527,L30,U283,L971,U199,L736,U36,R868,D297,L581,D888,L786,D865,R732,U394,L786,U838,L648,U434,L962,D862,R897,U116,L661,D848,L829,U930,L171,U959,R416,D855,L13,U941,R122,D678,R909,U536,R206,U39,L222,D501,L133,U360,R703,D928,R603,U793,L601,D935,R482,U444,L23,U331,L427,D349,L949,U147,L253,U757,R242,D307,R182,D371,L174,U518,L447,D851,R661,U432,R334,D240,R937,U625,L49,D105,R727,U504,L520,D126,R331,U176,L81,D168,L158,U774,L314,U623,R39,U743,R162,D646,R583,U523,R899,D419,L635,U958,R426,U482,L513,D624,L37,U669,L611,U167,L904,U163,L831,U222,L320,U561,R126,D7,L330,D313,R698,D473,R163,U527,R161,U823,L409,D734,L507,U277,L821,D341,R587,U902,R857,U386,R858,D522,R780,D754,L973,U1,R806,D439,R141,D621,R983,D546,R899,U566,L443,D147,R558,D820,R181,U351,R625,U60,R521,U225,R757,U673,L267,D624,L306,U531,L202,U854,L138,D725,R364,D813,L787,U183,R98,D899,R945,D363,L797
L993,D9,L41,D892,L493,D174,R20,D927,R263,D65,R476,D884,R60,D313,R175,U4,L957,U514,R821,U330,L973,U876,L856,D15,L988,U443,R205,D662,R753,U74,R270,D232,R56,D409,R2,U257,R198,U644,L435,U16,L914,D584,L909,D222,R919,U649,R77,U213,R949,D272,R893,U717,L939,U310,R637,D912,L347,D755,L895,D305,R460,D214,L826,D847,R680,U821,L688,U472,R721,U2,L755,D84,L716,U466,L833,U12,L410,D453,L462,D782,R59,U491,L235,D827,L924,U964,R443,D544,L904,D383,R259,D12,L538,D194,R945,U356,L85,D362,R672,D741,L556,U696,L994,U576,L201,D912,L282,D328,R322,D277,L269,U799,R150,U584,L479,U69,R313,U628,R114,D870,R660,D929,R964,U412,L790,U948,R949,D955,L555,U478,R967,D850,R569,D705,R30,U434,L948,U711,L507,D729,L256,U740,L60,D127,L95,U93,R260,D74,L267,D637,L658,U831,R882,D798,L173,U835,R960,D583,R411,U967,L515,U302,L456,D322,R963,U788,L516,U845,L131,U741,L246,D215,R233,U621,R420,D679,L8,D962,R514,U51,L891,U705,L699,U909,R408,D664,R324,U846,R503,U769,R32,D495,R154,U403,R145,U581,L708,D315,R556,U582,R363,U495,L722,U210,R718,U927,R994,D136,R744,U107,R316,D222,R796,U755,L69,D877,R661,D378,L215,D105,R333,D780,R335,D691,L263,U603,L582,U95,L140,D651,R414,D420,L497,U106,L470,D826,R706,D166,R500,D258,L225,U310,L866,U720,R247,D500,L340,U726,R296,U16,R227,U839,R537,U125,R700,U372,L310,D444,R214,D121,R151,U351,L767,D815,R537,U392,L595,U178,L961,D366,L216,U392,R645,U195,R231,D734,L441,D680,L226,D212,L142,U131,L427,D159,L538,D270,R553,D841,R115,U346,R673,D421,L403,D320,L296,U831,L655,U690,L105,U474,L687";

        /*
        --- Part Two ---
        It turns out that this circuit is very timing-sensitive;
        you actually need to minimize the signal delay.

        To do this, calculate the number of steps each wire takes to reach each intersection;
        */

        [Theory, AutoData]
        void Interpret_records_wire_length_from_origo(WirePathInterpreter i)
        {
            i.Interpret(
                (Direction.Down, 3),
                (Direction.Right, 3),
                (Direction.Up, 2));

            i.Grid.AsStringWithWireLengths().Should().Be(
                "......" + NL +
                "......" + NL +
                ".1..8." + NL +
                ".2..7." + NL +
                ".3456." + NL +
                "......" + NL);
        }

        [Theory, AutoData]
        void Interpret_records_minimum_wire_length_from_origo(WirePathInterpreter i)
        {
            i.Interpret(
                (Direction.Down, 3),
                (Direction.Right, 3),
                (Direction.Up, 1),
                (Direction.Left, 3));

            i.Grid.AsStringWithWireLengths().Should().Be(
                "......" + NL +
                "......" + NL +
                ".1...." + NL +
                ".2987." + NL +
                ".3456." + NL +
                "......" + NL);
        }

        /*
        choose the intersection where the sum of both wires' steps is lowest.
        If a wire visits a position on the grid multiple times,
        use the steps value from the first time it visits that position
        when calculating the total value of a specific intersection.

        The number of steps a wire takes is the total number of grid squares
        the wire has entered to get to that location,
        including the intersection being considered.
        Again consider the example from above:

        ...........
        .+-----+...
        .|.....|...
        .|..+--X-+.
        .|..|..|.|.
        .|.-X--+.|.
        .|..|....|.
        .|.......|.
        .o-------+.
        ...........
        In the above example,
        the intersection closest to the central port is reached after 8+5+5+2 = 20 steps
        by the first wire and 7+6+4+3 = 20 steps by the second wire for a total of 20+20 = 40 steps.

        However, the top-right intersection is better:
        the first wire takes only 8+5+2 = 15 and the second wire takes only 7+6+2 = 15,
        a total of 15+15 = 30 steps.

        Here are the best steps for the extra examples from above:

        R75,D30,R83,U83,L12,D49,R71,U7,L72
        U62,R66,U55,R34,D71,R55,D58,R83 = 610 steps
        R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
        U98,R91,D20,R16,D67,R40,U7,R15,U6,R7 = 410 steps
        What is the fewest combined steps the wires must take to reach an intersection?
         */

        [Theory]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72\r\nU62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\r\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        [InlineData(MyPuzzleInput, 21666)]
        void Fewest_combined_steps_the_wires_must_take_to_reach_an_intersection(
           string paths,
           int expected)
        {
            int distance = FewestCombinedStepsWiresTakeToReachIntersections(paths);

            distance.Should().Be(expected);
        }

        private int FewestCombinedStepsWiresTakeToReachIntersections(string paths)
        {
            var i = new WirePathInterpreter();
            var wireId = 1;
            foreach (var pathSegments in ParseWirePaths(paths))
            {
                i.Interpret(wireId, pathSegments.ToArray());
                wireId += 1;
            }

            var intersections = i.Grid.Intersections.ToArray();

            return
                i.Grid.CombinedWireLengths
                .Where(t => intersections.Contains((t.x,t.y)))
                .Min(t => t.wireLengthFromOrigo);
        }
    }
}
