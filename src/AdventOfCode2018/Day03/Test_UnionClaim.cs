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
        public void Has_maximum_bottom_right_x(Claim[] claims)
        {
            new UnionClaim(claims)
                .BottomRightX
                .Should().Be(
                    claims.Select(c => c.BottomRightX).Max());
        }

        /*     0    1    2    3  4  5
         *     C1   A1   B1   C2 A2 B2
         *          A------------*     A1 0
         *  Z     Z |    B-------+--*  B1 1
         *          |    |       |  |
         *  C1 C----+----+----*  |  |  C1 2
         *  X  |    |    |  X |  |  |
         *  B2 |    |    *----+--+--B  B2 3
         *  Y  |    |  Y      |  |
         *     |    |         |  |
         *  C2 *----+---------C  |     C2 4
         *          *------------A     A2 5
         *     C1 Z A1 Y B1 X C2
         */

        [Theory, LimitedRandomNumericData(0, 100)]
        public void Is_the_sum_of_composed_claims(IFixture fixture)
        {
            var xs = fixture.CreateMany<int>(6).OrderBy(i => i).ToArray();
            var ys = fixture.CreateMany<int>(6).OrderBy(i => i).ToArray();

            var a = (topLeft: (x: xs[1], y: ys[0]), bottomRight: (x: xs[4], y: ys[5]));
            var b = (topLeft: (x: xs[2], y: ys[1]), bottomRight: (x: xs[5], y: ys[3]));
            var c = (topLeft: (x: xs[0], y: ys[2]), bottomRight: (x: xs[3], y: ys[4]));

            var claimA = new Claim(a.topLeft, a.bottomRight);
            var claimB = new Claim(b.topLeft, b.bottomRight);
            var claimC = new Claim(c.topLeft, c.bottomRight);

            var union = new UnionClaim(claimA, claimB, claimC);

            var X = (x: fixture.CreateRandomBetween(b.topLeft.x, c.bottomRight.x - 1),
                     y: fixture.CreateRandomBetween(c.topLeft.y, b.bottomRight.y - 1));
            var Y = (x: fixture.CreateRandomBetween(a.topLeft.x, b.topLeft.x - 1),
                     y: fixture.CreateRandomBetween(b.bottomRight.y, c.bottomRight.y - 1));
            var Z = (x: fixture.CreateRandomBetween(0, a.topLeft.x - 1),
                     y: fixture.CreateRandomBetween(0, c.topLeft.y - 1));

            using (new AssertionScope())
            {
                union[X.x, X.y].Should().Be(3,
                    "because it is where 3 claims overlap" + FixtureToDebugString(X.x, X.y));
                union[Y.x, Y.y].Should().Be(2,
                    "because it is where 2 claims overlap" + FixtureToDebugString(Y.x, Y.y));
                union[Z.x, Z.y].Should().Be(0,
                    "because it is where no claims overlap" + FixtureToDebugString(Z.x, Z.y));
            }

            string FixtureToDebugString(int x, int y)
            {
                return
                    $" (x: {x}, y: {y}," +
                    $" A: {claimA.XOffset}, {claimA.YOffset}, {claimA.Width}, {claimA.Height}," +
                    $" B: {claimB.XOffset}, {claimB.YOffset}, {claimB.Width}, {claimB.Height}," +
                    $" C: {claimC.XOffset}, {claimC.YOffset}, {claimC.Width}, {claimC.Height})" +
                    union.ToDebugString((x, y));
            }
        }

        [Fact]
        public void Is_the_sum_of_composed_claims___x20()
        {
            for (int i = 0; i < 20; i++)
            {
                Is_the_sum_of_composed_claims(
                    LimitedRandomNumericDataAttribute.FixtureFactory(0, 100));
            }
        }
    }
}