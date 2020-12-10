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
        public void Can_parse_many_rules()
        {
            new RuleSet(Example).Rules.Take(3).Should().BeEquivalentTo(
                new Rule(
                    "light red",
                    "1 bright white",
                    "2 muted yellow"),
                new Rule(
                    "dark orange",
                    "3 bright white",
                    "4 muted yellow"),
                new Rule(
                    "bright white",
                    "1 shiny gold"));
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
        private static readonly IEnumerable<string> Separators = new[] {"bags contain", "bag,", "bags,", "bag.", "bags."};

        public RuleSet(string raw)
        {
            Rules = ParseRules(raw);
        }

        private static Rule[] ParseRules(string raw)
        {
            return raw.Split(Environment.NewLine)
                .Select(ParseRule)
                .ToArray();
        }

        private static Rule ParseRule(string line)
        {
            var tokens =
                line.Split(Separators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToArray();
            return new Rule(tokens[0], tokens.Skip(1).Select(t => t).ToArray());
        }

        public IEnumerable<Rule> Rules { get; }
    }
}