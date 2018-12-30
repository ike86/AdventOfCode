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
    }
}