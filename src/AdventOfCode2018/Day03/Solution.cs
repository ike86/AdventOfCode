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
        public static int GetNumberOfSquareInchesWithinTwoOrMoreClaims(string[] input)
        {
            var union = new UnionClaim(
                input
                .Select(i => Claim.Parse(i))
                .ToArray());
            var n = 0;
            for (int i = 0; i < union.XOffset + union.Width; i++)
            {
                for (int j = 0; j < union.YOffset + union.Height; j++)
                {
                    if (union[i, j] >= 2)
                    {
                        n += 1;
                    }
                }
            }

            return n;
        }

        [Fact(Skip = "long running")]
        public void _of_day_3_part_1()
        {
            var input = File.ReadAllLines("Day03/input.txt");

            var result = GetNumberOfSquareInchesWithinTwoOrMoreClaims(input);

            result.Should().Be(105231);
        }
    }
}