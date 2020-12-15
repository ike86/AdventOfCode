using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture.Xunit2;
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

        [Theory, AutoData]
        public void GetFirstNotSumOfPrevious_three_returns_first_outlier(int[] preamble, [Range(0, 9 - 3 - 1)] int i)
        {
            var possibleSums =
                preamble.Join(preamble, _ => 0, _ => 0, (a, b) => (a, b))
                    .Where(tuple => tuple.a != tuple.b)
                    .Select(tuple => tuple.a + tuple.b);
            possibleSums.Should().HaveCount(6);
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