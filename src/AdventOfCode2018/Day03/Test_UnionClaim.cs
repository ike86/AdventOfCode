using System;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
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
            x1 = Math
                .Min(
                    Math.Max(x1, claims[0].XOffset),
                    claims[0].XOffset + claims[0].Width);
            y1 = Math
                .Min(
                    Math.Max(y1, claims[0].YOffset),
                    claims[0].YOffset + claims[0].Height);
            var union = new UnionClaim(claims);

            union[x1, y1].Should().Be(claims.Select(c => c[x1, y1]).Sum());
        }
    }
}