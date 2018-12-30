using AutoFixture;

namespace AoC18.Day03
{
    internal static class FixtureExtensions
    {
        public static int CreateRandomBetween(this IFixture fixture, int from, int to)
        {
            if (from == to)
            {
                return (int)from;
            }

            return fixture.Build<int>()
                .FromFactory(new RandomNumericSequenceGenerator(from, to))
                .Create();
        }
    }
}