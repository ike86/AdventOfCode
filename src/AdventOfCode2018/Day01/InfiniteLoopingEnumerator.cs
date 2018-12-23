using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode2018.Day01
{
    internal class InfiniteLoopingEnumerator<TItem> : IEnumerator<TItem>
    {
        ////private readonly IEnumerable<TItem> source;
        private IEnumerator<TItem> enumerator;

        public InfiniteLoopingEnumerator(IEnumerable<TItem> source)
        {
            ////this.source = source;
            enumerator = source.GetEnumerator();
        }

        public TItem Current => throw new NotImplementedException();//// enumerator.Current;

        object IEnumerator.Current => throw new NotImplementedException();//// Current;

        public void Dispose()
        {
            ////enumerator?.Dispose();
            ////enumerator = null;
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
            ////if (b)
            ////{
            ////}
            ////else
            ////{
            ////    enumerator.Dispose();
            ////    enumerator = source.GetEnumerator();
            ////}

            ////return true;
        }

        public void Reset()
        {
            ////enumerator.Reset();
        }
    }
}