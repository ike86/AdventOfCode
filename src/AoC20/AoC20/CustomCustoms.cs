using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day06
    {
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
            Sum(ManyGroups).Should().Be(6);
        }
        
        [Fact(Skip = "not there yet")]
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
                    members => members.Skip(1)
                        .Aggregate(
                            seed: new HashSet<char>(members.First()),
                            (set, yesAnswers) =>
                            {
                                set.IntersectWith(new HashSet<char>(yesAnswers));
                                return set;
                            })
                        .Count);
        }
    }
}