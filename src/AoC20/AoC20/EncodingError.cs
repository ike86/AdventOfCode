using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day09
    {
        private const string Example =
            @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";
        
        [Fact(Skip = "This is a high level goal")]
        public void GetFirstNotSumOfPrevious_five_returns_expected_for_the_example()
        {
            new Decoder(Example).GetFirstNotSumOfPrevious(5).Should().Be(127);
        }
    }

    public class Decoder
    {
        public Decoder(string raw)
        {
            
        }

        public int GetFirstNotSumOfPrevious(int n)
        {
            return default;
        }
    }
}