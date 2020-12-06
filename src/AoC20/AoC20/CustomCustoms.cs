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

        private const string ManyGroups =
            @"abc

a
b
c

ab
ac

a
a
a
a

b";
        
        [Fact]
        public void Many_groups()
        {
            Sum(ManyGroups).Should().Be(11);
        }
        
        [Fact]
        public void Solve_puzzle()
        {
            Sum(PuzzleInput.ForDay06).Should().Be(6590);
        }

        private int Sum(string raw)
        {
            var groups =
                raw.Split(Environment.NewLine)
                    .Aggregate(
                        seed: new List<List<string>> {new List<string>()},
                        (ll, line) =>
                        {
                            if (line == string.Empty)
                            {
                                ll.Add(new List<string>());
                            }
                            else
                            {
                                ll.Last().Add(line);
                            }

                            return ll;
                        });

            return
                groups.Sum(
                    members => members.Aggregate(
                            seed: new HashSet<char>(),
                            (set, yesAnswers) =>
                            {
                                set.UnionWith(new HashSet<char>(yesAnswers));
                                return set;
                            })
                        .Count);
        }
    }
}