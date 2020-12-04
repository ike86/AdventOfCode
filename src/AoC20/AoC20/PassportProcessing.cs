using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Can_parse_first_passport()
        {
            IEnumerable<Passport> passports = Parse(Example);

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
        }

        private IEnumerable<Passport> Parse(string batch)
        {
            var l = batch.Split(Environment.NewLine).First();
            var l2 = batch.Split(Environment.NewLine).Skip(1).First();
            yield return
                new Passport(
                    ParseLineOfPassport(l)
                        .Concat(ParseLineOfPassport(l2)));

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
        public Passport(IEnumerable<(string key, string value)> kvps)
        {
            foreach (var kvp in kvps)
            {
                this.Add(kvp.key, kvp.value);
            }
        }
    }

    public class PassportProcessing
    {
    }
}