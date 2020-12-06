using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day06
    {
        [Fact]
        public void Group_of_one()
        {
            Sum("abcx").Should().Be(4);
        }
        
        private const string GroupOfMany = @"abcx
abcy
abcz";
        
        [Fact]
        public void Group_of_many()
        {
            Sum(GroupOfMany).Should().Be(6);
        }

        private int Sum(string raw)
        {
            var yesAnswersOfGroupMembers = raw.Split(Environment.NewLine);
            return
                yesAnswersOfGroupMembers.Aggregate(
                        seed: new HashSet<char>(),
                        (set, yesAnswers) =>
                        {
                            set.UnionWith(new HashSet<char>(yesAnswers));
                            return set;
                        })
                    .Count;
        }
    }
}