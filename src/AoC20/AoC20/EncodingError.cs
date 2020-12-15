using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AoC20.Day09
{
    public class Test
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
        public void GetFirstNotSumOfPrevious_three_returns_first_outlier(int[] preamble)
        {
            var possibleSums =
                preamble.GetAntiReflexivePairs()
                    .Select(tuple => tuple.a + tuple.b)
                    .ToArray();
            var impossibleSum = possibleSums.Max() + 1;

            new Decoder(preamble.Append(impossibleSum)).GetFirstNotSumOfPrevious(3)
                .Should().Be(impossibleSum);
        }

        private const int Many = 3;
        
        [Theory, AutoData]
        public void GetFirstNotSumOfPrevious_three_returns_first_outlier_2(
            int[] preamble,
            [Range(0, Many * Many - Many - 1)] int sumIndex)
        {
            var possibleSums =
                preamble.GetAntiReflexivePairs()
                    .Select(tuple => tuple.a + tuple.b)
                    .ToArray();
            var numbers = preamble.Append(possibleSums[sumIndex]).ToArray();
            
            possibleSums = numbers.GetAntiReflexivePairs()
                .Select(tuple => tuple.a + tuple.b)
                .ToArray();
            var impossibleSum = possibleSums.Max() + 1;
            numbers = numbers.Append(impossibleSum).ToArray();

            numbers.Should().HaveCount(Many + 2);
            new Decoder(numbers).GetFirstNotSumOfPrevious(3)
                .Should().Be(impossibleSum);
        }
    }

    public class Decoder
    {
        private readonly IEnumerable<int> _numbers;

        public Decoder(string raw)
        {
            
        }

        public Decoder(IEnumerable<int> numbers)
        {
            _numbers = numbers.ToArray();
        }

        public int GetFirstNotSumOfPrevious(int n)
        {
            for (int i = 0; i < _numbers.Count() - n; i++)
            {
                var prevN = _numbers.Skip(i).Take(n).ToArray();
                var current = _numbers.ElementAt(n + i);
                if (prevN.GetAntiReflexivePairs()
                        .None(tuple => tuple.a + tuple.b == current))
                {
                    return current;
                }
            }

            throw new InvalidOperationException("Could not find an outlier.");
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<(int a, int b)> GetAntiReflexivePairs(this IEnumerable<int> source)
        {
            source = source.ToArray();
            return
                source.Join(
                        source,
                        _ => 0,
                        _ => 0,
                        (a, b) => (a, b))
                    .Where(tuple => tuple.a != tuple.b);
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
            => !source.Any(predicate);
    }
}