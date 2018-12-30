using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC18.Day03
{
    public class Test_UnionClaim
    {
        [Theory, AutoData]
        public void Is_idempotent(Claim claim)
        {
            var union = new UnionClaim(claim);

            union.Should().BeEquivalentTo(claim);
        }

        [Theory, AutoData]
        public void Has_minimal_x_offset(Claim[] claims)
        {
            new UnionClaim(claims)
                .XOffset
                .Should().Be(
                    claims.Select(c => c.XOffset).Min());
        }

        [Theory, AutoData]
        public void Has_minimal_y_offset(Claim[] claims)
        {
            new UnionClaim(claims)
                .YOffset
                .Should().Be(
                    claims.Select(c => c.YOffset).Min());
        }

        [Theory, AutoData]
        public void Has_maximum_width(Claim[] claims)
        {
            new UnionClaim(claims)
                .Width
                .Should().Be(
                    claims.Select(c => c.Width).Max());
        }

        [Theory, AutoData]
        public void Has_maximum_height(Claim[] claims)
        {
            new UnionClaim(claims)
                .Height
                .Should().Be(
                    claims.Select(c => c.Height).Max());
        }

        [Theory, AutoData]
        public void Is_the_same_as_composed_claims(
            Claim[] claims,
            int x1, int y1,
            int x2, int y2,
            int x3, int y3)
        {
            x1 = x1.OrHorizontallyInboundOf(claims[0]);
            y1 = y1.OrVerticallyInboundOf(claims[0]);

            x2 = x1.OrHorizontallyInboundOf(claims[1]);
            y2 = y1.OrVerticallyInboundOf(claims[1]);

            var union = new UnionClaim(claims);

            using (new AssertionScope())
            {
                union[x1, y1].Should().Be(
                    claims.Select(c => c[x1, y1]).Sum());
                union[x2, y2].Should().Be(
                    claims.Select(c => c[x2, y2]).Sum());
                union[x3, y3].Should().Be(
                    claims.Select(c => c[x3, y3]).Sum());
            }
        }

        /*     0  1    2    3  4  5
         *     C1 A1   B1   C2 A2 B2
         *        A------------*     A1 0
         *        |    B-------+--*  B1 1
         *        |    |       |  |
         *  C1 C--+----+----*  |  |  C1 2
         *  X  |  |    |  X |  |  |
         *  B2 |  |    *----+--+--B  B2 3
         *  Y  |  |  Y      |  |
         *     |  |         |  |
         *  C2 *--+---------C  |     C2 4
         *        *------------A     A2 5
         *        A1 Y B1 X C2
         */

        [Theory, LimitedRandomNumericData(0, 100)]
        public void Is_the_sum_of_composed_claims(IFixture fixture)
        {
            var xs = fixture.CreateMany<int>(6).OrderBy(i => i).ToArray();
            var ys = fixture.CreateMany<int>(6).OrderBy(i => i).ToArray();

            var a = (topLeft: (xs[1], ys[0]), bottomRight: (xs[4], ys[5]));
            var b = (topLeft: (xs[2], ys[1]), bottomRight: (xs[5], ys[3]));
            var c = (topLeft: (xs[0], ys[2]), bottomRight: (xs[3], ys[4]));

            var claimA = new Claim(a.topLeft, a.bottomRight);
            var claimB = new Claim(b.topLeft, b.bottomRight);
            var claimC = new Claim(c.topLeft, c.bottomRight);

            var union = new UnionClaim(claimA, claimB, claimC);

            var X_x = fixture.CreateRandomBetween(b.topLeft.Item1, c.bottomRight.Item1 -1);
            var X_y = fixture.CreateRandomBetween(c.topLeft.Item2, b.bottomRight.Item2 -1);

            var Y_x = fixture.CreateRandomBetween(a.topLeft.Item1, b.topLeft.Item1 -1);
            var Y_y = fixture.CreateRandomBetween(b.bottomRight.Item2, c.bottomRight.Item2 - 1);

            using (new AssertionScope())
            {
                union[X_x, X_y].Should().Be(3,
                    "because it is where 3 claims overlap" +
                    $" ({nameof(X_x)}: {X_x}, {nameof(X_y)}: {X_y}," +
                    $" A: {claimA.XOffset}, {claimA.YOffset}, {claimA.Width}, {claimA.Height}," +
                    $" B: {claimB.XOffset}, {claimB.YOffset}, {claimB.Width}, {claimB.Height}," +
                    $" C: {claimC.XOffset}, {claimC.YOffset}, {claimC.Width}, {claimC.Height})" +
                    union.ToDebugString((X_x, X_y)));
                union[Y_x, Y_y].Should().Be(2, "because it is where 2 claims overlap");
            }
        }

        [Fact]
        public void Is_the_sum_of_composed_claims_x10()
        {
            for (int i = 0; i < 10; i++)
            {
                Is_the_sum_of_composed_claims(
                    LimitedRandomNumericDataAttribute.FixtureFactory(0, 100));
            }
        }
    }

    internal sealed class LimitedRandomNumericDataAttribute : AutoDataAttribute
    {
        public LimitedRandomNumericDataAttribute(long from, long to)
            : base(() => FixtureFactory(from, to))
        {
            From = from;
            To = to;
        }

        public long From { get; }

        public long To { get; }

        public static Func<long, long, IFixture> FixtureFactory =
            (long from, long to) =>
                new Fixture()
                    .Customize(
                        new RandomNumericSequenceGenerator(from, to)
                        .ToCustomization());
    }

    internal static class FixtureExtensions
    {
        public static int CreateRandomBetween(this IFixture fixture, int from, int to)
        {
            if (from == to)
            {
                return (int)from;
            }

            return fixture.Build<int>()
                .FromFactory(new RandomNumericSequenceGenerator(from, to))
                .Create();
        }
    }
}