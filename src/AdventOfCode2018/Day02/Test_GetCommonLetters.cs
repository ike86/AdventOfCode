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

        [Fact]
        public void Returns_character_for_two_matcing_characters()
        {
            var result = GetCommonLetters("a", "a");

            result.Should().Be("a");
        }

        /// <remarks>
        /// Distance is <see href="https://en.wikipedia.org/wiki/Hamming_distance">Hamming distance</see>
        /// </remarks>
        [Fact]
        public void Returns_one_common_letter_for_strings_one_distance_away()
        {
            var result = GetCommonLetters("ab", "cb");

            result.Should().Be("b");
        }
    }
}