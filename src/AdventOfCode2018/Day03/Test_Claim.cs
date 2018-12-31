using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC18.Day03
{
    public class Test_Claim
    {
        [Theory, AutoData]
        public void Parse_returns_claim_with_proper_attributes(
            int id,
            int xOffset,
            int yOffset,
            int width,
            int height)
        {
            var claim = Claim.Parse($"#{id} @ {xOffset},{yOffset}: {width}x{height}");

            using (new AssertionScope())
            {
                claim.XOffset.Should().Be(xOffset);
                claim.YOffset.Should().Be(yOffset);
                claim.Width.Should().Be(width);
                claim.Height.Should().Be(height);
                claim.BottomRightX.Should().Be(xOffset + width);
                claim.BottomRightY.Should().Be(yOffset + height);
                claim.Id.Should().Be(id);
            }
        }

        [Theory, AutoData]
        public void Is_one_between_its_bounds(int id, int xOffset, int yOffset, int width, int height)
        {
            var claim = new Claim(id, xOffset, yOffset, width, height);

            using (new AssertionScope())
            {
                for (int x = xOffset; x < xOffset + width; x++)
                {
                    for (int y = yOffset; y < yOffset + height; y++)
                    {
                        claim[x, y].Should().Be(1);
                    }
                }
            }
        }

        [Theory, AutoData]
        public void Is_zero_outside_its_bounds(
            Claim claim,
            int x1,
            int y1,
            int x2,
            int y2)
        {
            x1 = Math.Min(x1, claim.XOffset - 1);
            y1 = Math.Min(y1, claim.YOffset - 1);
            x2 = Math.Max(x2, claim.XOffset + claim.Width);
            y2 = Math.Max(y2, claim.YOffset + claim.Height);

            using (new AssertionScope())
            {
                claim[x1, y1].Should().Be(0);
                claim[x2, y2].Should().Be(0);
            }
        }

        [Theory, AutoData]
        public void ctor_sets_properties(
            int id,
            (int x, int y) topLeft,
            (int x, int y) bottomRight)
        {
            var claim = new Claim(id, topLeft, bottomRight);

            using (new AssertionScope())
            {
                claim.XOffset.Should().Be(topLeft.x);
                claim.YOffset.Should().Be(topLeft.y);
                claim.Width.Should().Be(bottomRight.x - topLeft.x);
                claim.Height.Should().Be(bottomRight.y - topLeft.y);
                claim.BottomRightX.Should().Be(bottomRight.x);
                claim.BottomRightY.Should().Be(bottomRight.y);
            }
        }
    }
}