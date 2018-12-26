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
    }
}