using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day04
    {
        private const string Example =
@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";

        [Fact]
        public void Can_parse_many_passports()
        {
            IEnumerable<Passport> passports = Parse(Example).ToArray();

            passports.First().Should().BeEquivalentTo(
                new Dictionary<string, string>
                {
                    ["ecl"] = "gry",
                    ["pid"] = "860033327",
                    ["eyr"] = "2020",
                    ["hcl"] = "#fffffd",
                    ["byr"] = "1937",
                    ["iyr"] = "2017",
                    ["cid"] = "147",
                    ["hgt"] = "183cm",
                });
            passports.Should().HaveCount(4);
        }

        [Fact]
        public void Can_count_valid_passports()
        {
            IEnumerable<Passport> passports = Parse(Example).ToArray();

            passports.Count(p => p.IsValid()).Should().Be(2);
        }

        private const string InvalidPassports =
            @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";
        
        [Fact]
        public void Invalid_passports()
        {
            IEnumerable<Passport> passports = Parse(InvalidPassports).ToArray();

            passports.Should().NotContain(p => p.IsValid());
        }
        
        private const string ValidPassports =
            @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
        
        [Fact]
        public void Valid_passports()
        {
            IEnumerable<Passport> passports = Parse(ValidPassports).ToArray();

            passports.Should().OnlyContain(p => p.IsValid());
        }
        
        [Fact]
        public void Solve_puzzle()
        {
            IEnumerable<Passport> passports = Parse(PuzzleInput.ForDay04).ToArray();

            passports.Count(p => p.IsValid()).Should().Be(140);
        }

        private IEnumerable<Passport> Parse(string batch)
        {
            var lines = batch.Split(Environment.NewLine);
            var passportLines = Enumerable.Empty<string>();
            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    yield return
                        new Passport(
                            passportLines.SelectMany(ParseLineOfPassport));
                    passportLines = Enumerable.Empty<string>();
                }
                else
                {
                    passportLines = passportLines.Append(line);
                }
            }

            if (passportLines.Any())
            {
                yield return
                    new Passport(
                        passportLines.SelectMany(ParseLineOfPassport));
            }

            IEnumerable<(string key, string value)> ParseLineOfPassport(string s)
            {
                return s.Split(" ")
                    .Select(kvp =>
                    {
                        var tokens = kvp.Split(":");
                        return (key: tokens[0], value: tokens[1]);
                    });
            }
        }
    }

    public class Passport : Dictionary<string, string>
    {
        private const string BirthYear = "byr";
        private const string IssueYear = "iyr";
        private const string ExpirationYear = "eyr";
        private const string Height= "hgt";
        private const string HairColor = "hcl";
        private const string EyeColor = "ecl";
        private const string PassportID = "pid";
        private const string CountryID = "cid";

        private static readonly IEnumerable<string> _eyeColors =
            new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

        private readonly Dictionary<string, Func<string, bool>> _validatorOf =
            new Dictionary<string, Func<string, bool>>
            {
                [BirthYear] = IsIntBetween(1920, 2020),
                [IssueYear] = IsIntBetween(2010, 2020),
                [ExpirationYear] = IsIntBetween(2020, 2030),
                [Height] = IsValidHeight(),
                [HairColor] = value => Regex.IsMatch(value, "^#[0-9a-f]{6}$"),
                [EyeColor] = value => _eyeColors.Any(c => value == c),
                [PassportID] = value => Regex.IsMatch(value, "^[0-9]{9}$"),
                [CountryID] = value => true,
            };

        public Passport(IEnumerable<(string key, string value)> kvps)
        {
            foreach (var kvp in kvps)
            {
                this.Add(kvp.key, kvp.value);
            }
        }

        public bool IsValid()
        {
            var expectedFieldsMissing =
                new[] {BirthYear, IssueYear, ExpirationYear, Height, HairColor, EyeColor, PassportID}
                    .Except(this.Keys)
                    .ToArray();

            return
                (IsEmpty(expectedFieldsMissing)
                   || (expectedFieldsMissing.Length == 1
                       && expectedFieldsMissing.Single() == CountryID))
                && this.Select(kvp => _validatorOf[kvp.Key](kvp.Value)).All(b => b);

            bool IsEmpty(string[] strings) => !strings.Any();
        }

        private static Func<string, bool> IsIntBetween(int min, int max) =>
            value =>
            {
                if (int.TryParse(value, out var i))
                {
                    return min <= i && i <= max;
                }

                return false;
            };

        private static Func<string, bool> IsValidHeight() =>
            value =>
            {
                const string cm = "cm";
                if (value.EndsWith(cm))
                {
                    var number = value.Replace(cm, string.Empty);
                    return IsIntBetween(150, 193)(number);
                }

                const string inch = "in";
                if (value.EndsWith(inch))
                {
                    var number = value.Replace(inch, string.Empty);
                    return IsIntBetween(59, 76)(number);
                }

                return false;
            };
    }
}