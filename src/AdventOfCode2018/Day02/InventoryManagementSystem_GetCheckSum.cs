﻿using FluentAssertions;
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
        [InlineData("aabcdef")]
        [InlineData("aaabcdef")]
        [InlineData("aabbcdef")]
        public void Returns_one_for_single_input_with_some_characters_appearing_twice(string input)
        {
            var checkSum = InventoryManagementSystem.GetCheckSum(input);

            checkSum.Should().Be(1);
        }
    }
}