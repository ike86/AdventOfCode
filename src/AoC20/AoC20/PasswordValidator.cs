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
        
        [Fact]
        public void Test_part_2_example()
        {
            var records = ParseForTobogganRental(Example);

            records.Count(r => r.IsValid()).Should().Be(1);
        }
        
        [Fact]
        public void Solve_part_2_puzzle()
        {
            var records = ParseForTobogganRental(PuzzleInput.ForDay02);

            records.Count(r => r.IsValid()).Should().Be(729);
        }

        private static IEnumerable<DatabaseRecord> ParseForShedRentalPlace(string raw)
        {
            return
                raw.Split(Environment.NewLine)
                .Select(ParseShedRentalLine);

            static DatabaseRecord ParseShedRentalLine(string line) =>
                ParseLine(
                    line,
                    (min, max, letter) => new ShedRentalPolicy(min, max, letter));
        }
        
        private IEnumerable<DatabaseRecord> ParseForTobogganRental(string raw)
        {
            return
                raw.Split(Environment.NewLine)
                .Select(ParseTobogganLine);

            static DatabaseRecord ParseTobogganLine(string line) =>
                ParseLine(
                    line,
                    (i1, i2, letter) => new TobogganRentalPolicy(i1, i2, letter));
        }

        private static DatabaseRecord ParseLine(string line, Func<int, int,char, IPolicy> createPolicy)
        {
            var tokens = line.Split(new[] {'-', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
            var i1 = int.Parse(tokens[0]);
            var i2 = int.Parse(tokens[1]);
            var letter = tokens[2].Single();
            var password = tokens[3];

            return
                new DatabaseRecord(
                    createPolicy(i1, i2, letter),
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
        
        public class TobogganRentalPolicy : IPolicy
        {
            public TobogganRentalPolicy(int position1, int position2, char letter)
            {
                Position1 = position1;
                Position2 = position2;
                Letter = letter;
            }

            public int Position1 { get; }

            public int Position2 { get; }

            public char Letter { get; }

            public bool IsCompliant(string password)
            {
                if (password[Position1 - 1] == Letter
                    && password[Position2 - 1] != Letter
                    || password[Position1 - 1] != Letter
                    && password[Position2 - 1] == Letter)
                {
                    return true;
                }

                return false;
            }
        }
    }
}