﻿using System;
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
                    (1, "bright white"),
                    (2, "muted yellow")),
                new Rule(
                    "dark orange",
                    (3, "bright white"),
                    (4, "muted yellow")),
                new Rule(
                    "bright white",
                    (1, "shiny gold")));
        }

        [Fact]
        public void Can_parse_terminal_rule()
        {
            new RuleSet("dotted black bags contain no other bags.").Rules.Should().BeEquivalentTo(
                new Rule("dotted black"));
        }

        [Theory]
        [InlineData("bright white")]
        [InlineData("muted yellow")]
        public void Can_contain_shiny_gold(string color)
        {
            new RuleSet(Example).CanContainShinyGold(color).Should().BeTrue();
        }
    }

    public class Rule
    {
        public Rule(string outerColor, params (int number, string color)[] allowedBags)
        {
            OuterColor = outerColor;
            AllowedBags = allowedBags;
        }

        public string OuterColor { get; }

        public IEnumerable<(int number, string color)> AllowedBags { get; }
    }

    public class RuleSet
    {
        private static readonly IEnumerable<string> Separators = new[] {"bags contain", "bag,", "bags,", "bag.", "bags."};

        public RuleSet(string raw)
        {
            Rules = ParseRules(raw);
        }

        public IEnumerable<Rule> Rules { get; }

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

            return new Rule(tokens[0], ParseAllowedBags(tokens));
        }

        private static (int, string t)[] ParseAllowedBags(string[] tokens)
        {
            if (tokens[1] == "no other")
            {
                return Enumerable.Empty<(int, string)>().ToArray();
            }
            
            return tokens.Skip(1)
                .Select(t => ParseNumberAndColor(t))
                .ToArray();
        }

        private static (int, string t) ParseNumberAndColor(string s)
        {
            var tokens = s.Split(' ');
            
            return (int.Parse(tokens[0]), string.Join(' ', tokens.Skip(1)));
        }

        public bool CanContainShinyGold(string color)
        {
            return
                Rules.FirstOrDefault(rule => rule.OuterColor == color)
                ?.AllowedBags.Any(t => t.color == "shiny gold")
                ?? false;
        }
    }
}