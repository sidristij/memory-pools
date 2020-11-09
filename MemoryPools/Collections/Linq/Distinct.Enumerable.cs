using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    internal class DistinctExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        private IPoolingEnumerator<T> _parent;
        private IEqualityComparer<T> _comparer;

        public DistinctExprEnumerable<T> Init(IPoolingEnumerator<T> parent, IEqualityComparer<T> comparer  = default)
        {
            _parent = parent;
            _comparer = comparer;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<DistinctExprEnumerator>().Init(this, _parent, _comparer);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _parent?.Dispose();
                _parent = default;
                Pool.Return(this);
            }
        }
        internal class DistinctExprEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator<T> _src;
            private PoolingDictionary<T, int> _hashset;
            private DistinctExprEnumerable<T> _parent;
            
            public DistinctExprEnumerator Init(
                DistinctExprEnumerable<T> parent,
                IPoolingEnumerator<T> src,
                IEqualityComparer<T> comparer)
            {
                _src = src;
                _parent = parent;
                _hashset = Pool.Get<PoolingDictionary<T, int>>().Init(0, comparer ?? EqualityComparer<T>.Default);
                return this;
            }

            public bool MoveNext()
            {
                while (_src.MoveNext())
                {
                    if(_hashset.ContainsKey(_src.Current)) continue;

                    _hashset[_src.Current] = 1;
                    return true;
                }

                return false;
            }

            public void Reset() => _src.Reset();

            object IPoolingEnumerator.Current => Current;

            public T Current => _src.Current;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = default;
                
                _hashset?.Dispose();
                _hashset = default;
                
                _src = default;
                Pool.Return(this);
            }
        }
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}