using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Fact]
        public void _of_day_3_part_1()
        {
            var input = File.ReadAllLines("Day03/input.txt");

            var result = Solve(input);

            result.Should().Be(105231);
        }
    }
}
