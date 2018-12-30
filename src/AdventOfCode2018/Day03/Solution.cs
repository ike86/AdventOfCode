using System;
using System.IO;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AoC18.Day03
{
    public class Solution
    {
        public static int Solve(string[] input)
        {
            var union = new UnionClaim(
                input
                .Select(i => Claim.Parse(i))
                .ToArray());
            var numberOfSquareInchesWithinTwoOrMoreClaims = 0;
            for (int i = 0; i < union.XOffset + union.Width; i++)
            {
                for (int j = 0; j < union.YOffset + union.Height; j++)
                {
                    if (union[i, j] >= 2)
                    {
                        numberOfSquareInchesWithinTwoOrMoreClaims += 1;
                    }
                }
            }

            return numberOfSquareInchesWithinTwoOrMoreClaims;
        }

        [Fact(Skip = "long running")]
        public void _of_day_3_part_1()
        {
            var input = File.ReadAllLines("Day03/input.txt");

            var result = Solve(input);

            result.Should().Be(105231);
        }

        [Theory, AutoData]
        public void Solution2_returns_id_of_single_claim(Claim claim)
        {
            var part2solver = new Part2Solver(claim);

            var id = part2solver.GetIdOfOnlyNonOverlappingClaim();

            id.Should().Be(claim.Id);
        }

        public static int SolvePart2(string[] input)
        {
            var claims = input
                .Select(i => Claim.Parse(i))
                .ToArray();
            var union = new UnionClaim(claims);

            foreach (var claim in claims)
            {
                var isMatching = true;
                for (int i = claim.XOffset; i < claim.BottomRightX; i++)
                {
                    for (int j = claim.YOffset; j < claim.BottomRightY; j++)
                    {
                        if (isMatching && union[i, j] >= 2)
                        {
                            isMatching = false;
                        }
                    }
                }

                if (isMatching)
                {
                    return claim.Id;
                }
            }

            throw new ArgumentException();
        }
    }
}