using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class PasswordValidator
    {
        private const string Example = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";

        [Fact]
        public void Test_parse_many()
        {
            var records =
                Parse(Example)
                    .ToArray();

            records.Should().HaveCount(3);
            records.Should().BeEquivalentTo(
                new DatabaseRecord(
                    new Policy(1, 3, 'a'),
                    "abcde"),
                new DatabaseRecord(
                    new Policy(1, 3, 'b'),
                    "cdefg"),
                new DatabaseRecord(
                    new Policy(2, 9, 'c'),
                    "ccccccccc"));
        }

        [Fact]
        public void Test()
        {
            var records = Parse(Example);

            records.Count(r => r.IsValid()).Should().Be(2);
        }

        private IEnumerable<DatabaseRecord> Parse(string raw)
        {
            return raw.Split(Environment.NewLine)
                .Select(line => ParseLine(line));
        }

        private DatabaseRecord ParseLine(string line)
        {
            var tokens = line.Split(new[] {'-', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
            var min = int.Parse(tokens[0]);
            var max = int.Parse(tokens[1]);
            var letter = tokens[2].Single();
            var password = tokens[3];

            return
                new DatabaseRecord(
                    new Policy(min, max, letter),
                    password);
        }

        public class DatabaseRecord
        {
            public DatabaseRecord(Policy policy, string password)
            {
                Policy = policy;
                Password = password;
            }

            public Policy Policy { get; }

            public string Password { get; }

            public bool IsValid()
            {
                return false;
            }
        }

        public class Policy
        {
            public Policy(int min, int max, char letter)
            {
                Min = min;
                Max = max;
                Letter = letter;
            }

            public int Min { get; }

            public int Max { get; }

            public char Letter { get; }
        }
    }
}