using FluentAssertions;
using Xunit;
using static AoC18.Day02.Part2;

namespace AoC18.Day02
{
    public class GetCommonLetters
    {
        [Fact]
        public void Returns_empty_for_two_different_strings()
        {
            var result = GetCommonLetters("abcde", "fghij");

            result.Should().Be(string.Empty);
        }

        [Fact]
        public void Returns_empty_for_strings_differing_at_more_than_one_positions()
        {
            var result = GetCommonLetters("abcde", "aBcdE");

            result.Should().Be(string.Empty);
        }

        [Theory]
        [InlineData("abcde", "aBcde", "acde")]
        [InlineData("abcde", "aBcdE", "")]
        public void Returns_common_letters_only_for_strings_differing_at_one_position(
            string first,
            string second,
            string commonLetters)
        {
            GetCommonLetters(first, second).Should().Be(commonLetters);
        }
    }
}