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
            int xOffset,
            int yOffset,
            int width,
            int height)
        {
            var claim = Claim.Parse($"#1 @ {xOffset},{yOffset}: {width}x{height}");

            using (new AssertionScope())
            {
                claim.XOffset.Should().Be(xOffset);
                claim.YOffset.Should().Be(yOffset);
                claim.Width.Should().Be(width);
                claim.Height.Should().Be(height);
            }
        }

        [Theory, AutoData]
        public void Parse_respects_x_offset(int xOffset)
        {
            var claim = Claim.Parse($"#1 @ {xOffset},3: 4x4");

            using (new AssertionScope())
            {
                claim[0, 3].Should().Be(0);
                claim[xOffset, 3].Should().Be(1);
            }
        }

        [Theory, AutoData]
        public void Parse_respects_y_offset(int yOffset)
        {
            var claim = Claim.Parse($"#1 @ 1,{yOffset}: 4x4");

            using (new AssertionScope())
            {
                claim[1, 0].Should().Be(0);
                claim[1, yOffset].Should().Be(1);
            }
        }

        [Theory, AutoData]
        public void Parse_respects_width(int xOffset, int width)
        {
            var claim = Claim.Parse($"#1 @ {xOffset},3: {width}x4");

            using (new AssertionScope())
            {
                claim[xOffset + width - 1, 3].Should().Be(1);
                claim[xOffset + width, 3].Should().Be(0);
            }
        }

        [Theory, AutoData]
        public void Parse_respects_height(int yOffset, int height)
        {
            var claim = Claim.Parse($"#1 @ 1,{yOffset}: 4x{height}");

            using (new AssertionScope())
            {
                claim[1, yOffset + height - 1].Should().Be(1);
                claim[1, yOffset + height].Should().Be(0);
            }
        }

        [Theory, AutoData]
        public void Is_one_between_its_bounds(int xOffset, int yOffset, int width, int height)
        {
            var claim = new Claim(xOffset, yOffset, width, height);

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
    }
}