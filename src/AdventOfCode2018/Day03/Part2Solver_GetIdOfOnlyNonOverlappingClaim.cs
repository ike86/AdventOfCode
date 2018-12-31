using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AoC18.Day03
{
    public class Part2Solver_GetIdOfOnlyNonOverlappingClaim
    {
        [Theory, AutoData]
        public void Returns_id_of_single_claim(Claim claim)
        {
            var part2solver = new Part2Solver(claim);

            var id = part2solver.GetIdOfOnlyNonOverlappingClaim();

            id.Should().Be(claim.Id);
        }

        /*  0    1    2    3  4  5  6  7
         *  C1   A1   B1   C2 A2 D1 B2 D2
         *       A------------*           A1 0
         *       |    B-------+-----*     B1 1
         *       |    |       |     |
         *  C----+----+----*  |     |     C1 2
         *  |    |    |    |  |     |
         *  |    |    *----+--+-----B     B2 3
         *  |    |         |  |  D-----*  D1 4
         *  |    |         |  |  |     |
         *  *----+---------C  |  |     |  C2 5
         *       *------------A  |     |  A2 6
         *                       *-----D  D2 7
         */

        [Theory, AutoData]
        public void Returns_id_of_non_overlapping_claim(IFixture fixture)
        {
            var xs = fixture.CreateMany<int>(8).OrderBy(i => i).ToArray();
            var ys = fixture.CreateMany<int>(8).OrderBy(i => i).ToArray();

            var a = (topLeft: (x: xs[1], y: ys[0]), bottomRight: (x: xs[4], y: ys[6]));
            var b = (topLeft: (x: xs[2], y: ys[1]), bottomRight: (x: xs[6], y: ys[3]));
            var c = (topLeft: (x: xs[0], y: ys[2]), bottomRight: (x: xs[3], y: ys[5]));
            var d = (topleft: (x: xs[5], y: ys[4]), bottomRight: (x: xs[7], y: ys[7]));

            var claimA = new Claim(fixture.Create<int>(), a.topLeft, a.bottomRight);
            var claimB = new Claim(fixture.Create<int>(), b.topLeft, b.bottomRight);
            var claimC = new Claim(fixture.Create<int>(), c.topLeft, c.bottomRight);
            var claimD = new Claim(fixture.Create<int>(), d.topleft, d.bottomRight);

            var part2solver = new Part2Solver(claimA, claimB, claimC, claimD);

            var id = part2solver.GetIdOfOnlyNonOverlappingClaim();

            id.Should().Be(claimD.Id);
        }
    }
}