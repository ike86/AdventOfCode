using System;
using AutoFixture;
using AutoFixture.Xunit2;

namespace AoC18.Day03
{
    internal sealed class LimitedRandomNumericDataAttribute : AutoDataAttribute
    {
        public LimitedRandomNumericDataAttribute(long from, long to)
            : base(() => FixtureFactory(from, to))
        {
            From = from;
            To = to;
        }

        public long From { get; }

        public long To { get; }

        public static Func<long, long, IFixture> FixtureFactory =
            (long from, long to) =>
                new Fixture()
                    .Customize(
                        new RandomNumericSequenceGenerator(from, to)
                        .ToCustomization());
    }
}