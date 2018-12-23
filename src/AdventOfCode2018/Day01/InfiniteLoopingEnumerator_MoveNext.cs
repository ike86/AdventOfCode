using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2018.Day01
{
    public class InfiniteLoopingEnumerator_MoveNext
    {
        [Theory, AutoData]
        public void Returns_false_on_empty_source(IFixture fixture)
        {
            fixture.Inject(Enumerable.Empty<int>());
            var enumerator = fixture.Create<InfiniteLoopingEnumerator<int>>();

            enumerator.MoveNext().Should().BeFalse();
        }
    }
}