using FluentAssertions;
using Xunit;

namespace AoC18.Day02
{
    public class InventoryManagementSystem_GetCheckSum
    {
        [Fact]
        public void Returns_zero_for_single_input_with_unique_characters()
        {
            var checkSum = InventoryManagementSystem.GetCheckSum("abcdef");

            checkSum.Should().Be(0);
        }

        [Fact]
        public void Returns_one_for_single_input_with_a_character_appearing_twice()
        {
            var checkSum = InventoryManagementSystem.GetCheckSum("aabcdef");

            checkSum.Should().Be(1);
        }
    }
}