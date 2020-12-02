using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day02
    {
        private const string Example = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";

        [Fact]
        public void Test_parse_many()
        {
            var records =
                ParseForShedRentalPlace(Example)
                    .ToArray();

            records.Should().HaveCount(3);
            records.Should().BeEquivalentTo(
                new DatabaseRecord(
                    new ShedRentalPolicy(1, 3, 'a'),
                    "abcde"),
                new DatabaseRecord(
                    new ShedRentalPolicy(1, 3, 'b'),
                    "cdefg"),
                new DatabaseRecord(
                    new ShedRentalPolicy(2, 9, 'c'),
                    "ccccccccc"));
        }
        
        [Fact]
        public void Valid_record_IsValid()
        {
            new DatabaseRecord(
                    new ShedRentalPolicy(1, 3, 'a'),
                    "abcde")
                .IsValid().Should().BeTrue();
        }
        
        [Fact]
        public void Valid_record_not_IsValid()
        {
            new DatabaseRecord(
                    new ShedRentalPolicy(1, 3, 'b'),
                    "cdefg")
                .IsValid().Should().BeFalse();
        }

        [Fact]
        public void Test_example()
        {
            var records = ParseForShedRentalPlace(Example);

            records.Count(r => r.IsValid()).Should().Be(2);
        }
        
        [Fact]
        public void Solve_puzzle()
        {
            var records = ParseForShedRentalPlace(PuzzleInput.ForDay02);

            records.Count(r => r.IsValid()).Should().Be(582);
        }

        private IEnumerable<DatabaseRecord> ParseForShedRentalPlace(string raw)
        {
            return raw.Split(Environment.NewLine)
                .Select(line => ParseLine(line, (min, max, letter) => new ShedRentalPolicy(min, max, letter)));
        }

        private DatabaseRecord ParseLine(string line, Func<int, int,char, IPolicy> createPolicy)
        {
            var tokens = line.Split(new[] {'-', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
            var min = int.Parse(tokens[0]);
            var max = int.Parse(tokens[1]);
            var letter = tokens[2].Single();
            var password = tokens[3];

            return
                new DatabaseRecord(
                    createPolicy(min, max, letter),
                    password);
        }

        public class DatabaseRecord
        {
            public DatabaseRecord(IPolicy policy, string password)
            {
                Policy = policy;
                Password = password;
            }

            public IPolicy Policy { get; }

            public string Password { get; }

            public bool IsValid()
            {
                return Policy.IsCompliant(Password);
            }
        }

        public interface IPolicy
        {
            bool IsCompliant(string password);
        }

        public class ShedRentalPolicy : IPolicy
        {
            public ShedRentalPolicy(int min, int max, char letter)
            {
                Min = min;
                Max = max;
                Letter = letter;
            }

            public int Min { get; }

            public int Max { get; }

            public char Letter { get; }

            public bool IsCompliant(string password)
            {
                var occurrences = password.Count(ch => ch == Letter);
                if (Min <= occurrences
                    && occurrences <= Max)
                {
                    return true;
                }

                return false;
            }
        }
    }
}