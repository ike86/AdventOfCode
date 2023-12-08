using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace AoC23;

public partial class Day01
{
    private readonly ITestOutputHelper testOutputHelper;

    public Day01(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Parse_returns_spelled_out_and_normal_digits()
    {
        Arb.Register<Generators>();
        Prop.ForAll<ITestToken[]>(
                xs =>
                {
                    var s = string.Join(string.Empty, xs.Select(x => x.ToString()));
                    Parse(s).Should().BeEquivalentTo(
                        xs.Where(x => x is TestSpelledOutDigit or TestDigit)
                            .Select(x => new { Value = x.Value }));
                })
            .VerboseCheckThrowOnFailure(testOutputHelper);
    }

    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void GetCalibrationValues_returns_first_and_last_digits_as_int()
    {
        Configuration.VerboseThrowOnFailure.StartSize = 1;
        Arb.Register<Generators>();
        Prop.ForAll<ITestToken[]>(
                xs =>
                {
                    var s = string.Join(string.Empty, xs.Select(x => x.ToString()));
                    var digits =
                        xs.Where(x => x is TestSpelledOutDigit or TestDigit)
                            .Select(x => x.Value)
                            .ToArray();
                    GetCalibrationValues(s).Should().BeEquivalentTo(
                        new[] { int.Parse($"{digits.First()}{digits.Last()}") });
                })
            .VerboseCheckThrowOnFailure(testOutputHelper);
    }

    public class Generators
    {
        [UsedImplicitly]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public static Arbitrary<ITestToken[]> ITestToken()
        {
            var noneGen =
                Gen.Two(
                        Gen.Elements(
                            AllChars().Where(ch => char.IsDigit(ch) is false)))
                    .Select(t => $"{t.Item1}{t.Item2}")
                    .Select(s => new TestGibberish(s))
                    .Select(x => x as ITestToken);

            var spelledOutDigitGen =
                Gen.Elements(
                        new TestSpelledOutDigit(1, "one"),
                        new TestSpelledOutDigit(2, "two"),
                        new TestSpelledOutDigit(3, "three"),
                        new TestSpelledOutDigit(4, "four"),
                        new TestSpelledOutDigit(5, "five"),
                        new TestSpelledOutDigit(6, "six"),
                        new TestSpelledOutDigit(7, "seven"),
                        new TestSpelledOutDigit(8, "eight"),
                        new TestSpelledOutDigit(9, "nine"))
                    .Select(x => x as ITestToken);

            var digitGen =
                Gen.Choose(1, 9)
                    .Select(i => new TestDigit(i))
                    .Select(x => x as ITestToken);
            return
                Arb.From(
                    Gen.ArrayOf(
                            Gen.OneOf(
                                noneGen,
                                spelledOutDigitGen,
                                digitGen))
                        .Where(xs => xs.Any(x => x is TestSpelledOutDigit or TestDigit)));
        }

        private static IEnumerable<char> AllChars()
        {
            for (int i = char.MinValue; i <= char.MaxValue; i++)
            {
                var c = Convert.ToChar(i);
                if (!char.IsControl(c))
                {
                    yield return c;
                }
            }
        }
    }

    public interface ITestToken
    {
        public int Value { get; }
    }

    public class TestSpelledOutDigit : ITestToken
    {
        private readonly string s;

        public TestSpelledOutDigit(int value, string s)
        {
            this.s = s;
            Value = value;
        }

        public int Value { get; }

        public override string ToString() => s;
    }

    public class TestDigit : ITestToken
    {
        public TestDigit(int digit)
        {
            Value = digit;
        }

        public int Value { get; }

        public override string ToString() => Value.ToString();
    }

    public class TestGibberish : ITestToken
    {
        private readonly string s;

        public TestGibberish(string s)
        {
            this.s = s;
        }

        public int Value => throw new NotImplementedException();

        public override string ToString() => s;
    }
}