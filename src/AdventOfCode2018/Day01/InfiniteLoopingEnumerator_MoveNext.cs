using System.Collections.Generic;
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
        public void Returns_false_for_empty_source(IFixture fixture)
        {
            fixture.Inject(Enumerable.Empty<int>());
            var enumerator = fixture.Create<InfiniteLoopingEnumerator<int>>();

            enumerator.MoveNext().Should().BeFalse();
        }

        [Theory, AutoData]
        internal void Returns_true_for_some_source(
            [Frozen]InfiniteLoopingEnumerator<int> enumerator)
        {
            enumerator.MoveNext().Should().BeTrue();
        }

        [Theory, AutoData]
        internal void Sets_current_to_first_element_of_source(
            [Frozen]IEnumerable<int> source,
            [Frozen]InfiniteLoopingEnumerator<int> enumerator)
        {
            enumerator.MoveNext();

            enumerator.Current.Should().Be(source.First());
        }

        [Theory, AutoData]
        internal void Enumerates_the_source(
            [Frozen]IEnumerable<int> source,
            [Frozen]InfiniteLoopingEnumerator<int> enumerator)
        {
            var result = Enumerable
                .Range(0, source.Count())
                .Select(_ =>
                {
                    enumerator.MoveNext();
                    return enumerator.Current;
                });

            result.Should().BeEquivalentTo(source);
        }

        [Theory, AutoData]
        internal void Enumerates_the_source_n_times(
            [Frozen]IEnumerable<int> source,
            [Frozen]InfiniteLoopingEnumerator<int> enumerator,
            int n)
        {
            var result = enumerator.AsEnumerable()
                .Take(source.Count() * n);

            result.Should().BeEquivalentTo(
                source.Multiply(times: n));
        }
    }
}