using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day07
    {
        private const string Example =
            @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";

        [Fact]
        public void Can_parse_a_rule()
        {
            new RuleSet(Example).Rules.Should().BeEquivalentTo(
                new Rule(
                    "light red",
                    "1 bright white",
                    "2 muted yellow"));
            // (number: 1, color: "bright white"),
            // (number: 2, color: "muted yellow")));
        }
    }

    public class Rule
    {
        public Rule(string outerColor, params string[] allowedBags)
        {
            OuterColor = outerColor;
            AllowedBags = allowedBags;
        }

        public string OuterColor { get; }

        public IEnumerable<string> AllowedBags { get; }
    }

    public class RuleSet
    {
        public RuleSet(string raw)
        {
            var tokens =
                raw.Split(Environment.NewLine)
                    .First()
                    .Split(new[] {"bags contain", "bag,", "bags,", "bag.", "bags."},
                        StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();
            Rules =
                new[]
                {
                    new Rule(tokens[0], tokens.Skip(1).Select(t => t).ToArray()),
                };
        }

        public IEnumerable<Rule> Rules { get; }
    }
}