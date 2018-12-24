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
    }
}