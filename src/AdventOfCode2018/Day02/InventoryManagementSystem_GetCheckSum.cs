using FluentAssertions;
using Xunit;

namespace AoC18.Day02
{
    public class GetCheckSum
    {
        [Fact]
        public void Returns_zero_for_single_input_with_unique_characters()
        {
            var checkSum = InventoryManagementSystem.GetCheckSum("abcdef");

            checkSum.Should().Be(0);
        }

        [Theory]
        [InlineData("AAbcd")]
        [InlineData("aBBcd")]
        [InlineData("AABBcde")]
        [InlineData("AAAbcd")]
        [InlineData("AAABBBcde")]
        public void Returns_zero_for_single_input_with_some_characters_appearing_twice_xor_three_times(string input)
        {
            var checkSum = InventoryManagementSystem.GetCheckSum(input);

            checkSum.Should().Be(0);
        }

        [Theory]
        [InlineData("AAABBcde")]
        [InlineData("AABBBcd")]
        [InlineData("AABBcdEEEf")]
        public void Returns_one_for_single_input_with_some_characters_appearing_twice_and_three_times(string input)
        {
            var checkSum = InventoryManagementSystem.GetCheckSum(input);

            checkSum.Should().Be(1);
        }

        [Fact]
        public void Returns_one_for_many_input()
        {
            var input =
                "abcdef" + InventoryManagementSystem.LineSeparator +
                "AAbcd" + InventoryManagementSystem.LineSeparator +
                "AAAbcd";

            var checkSum = InventoryManagementSystem.GetCheckSum(input);

            checkSum.Should().Be(1);
        }
    }
}