using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC18.Day03
{
    public class Test_Claim
    {
        [Fact]
        public void Parse_respects_x_offset()
        {
            var claim = Claim.Parse("#1 @ 1,3: 4x4");

            using (new AssertionScope())
            {
                claim[0, 3].Should().Be(0);
                claim[1, 3].Should().Be(1);
            }
        }

        [Fact]
        public void Parse_respects_y_offset()
        {
            var claim = Claim.Parse("#1 @ 1,3: 4x4");

            using (new AssertionScope())
            {
                claim[1, 0].Should().Be(0);
                claim[1, 3].Should().Be(1);
            }
        }
    }
}