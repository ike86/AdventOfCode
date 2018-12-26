using FluentAssertions;
using Xunit;
using static AoC18.Day02.Part2;

namespace AoC18.Day02
{
    public class GetCommonLetters
    {
        [Fact]
        public void Returns_empty_for_two_different_characters()
        {
            var result = GetCommonLetters("a", "b");

            result.Should().Be(string.Empty);
        }
    }
}