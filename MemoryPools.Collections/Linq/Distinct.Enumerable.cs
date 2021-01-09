using System;
using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    internal class DistinctExprEnumerable<T, TItem> : IPoolingEnumerable<T>
    {
        private int _count;
        private IPoolingEnumerator<T> _parent;
        private IEqualityComparer<TItem> _comparer;
        private Func<T, TItem> _selector;

        public DistinctExprEnumerable<T, TItem> Init(IPoolingEnumerator<T> parent, Func<T, TItem> selector, IEqualityComparer<TItem> comparer  = default)
        {
            _parent = parent;
            _selector = selector;
            _comparer = comparer;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool<DistinctExprEnumerator>.Get().Init(this, _parent, _selector, _comparer);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _parent?.Dispose();
                _parent = default;
                _selector = default;
                Pool<DistinctExprEnumerable<T, TItem>>.Return(this);
            }
        }
        internal class DistinctExprEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator<T> _src;
            private Func<T, TItem> _selector;
            private PoolingDictionary<TItem, int> _hashset;
            private DistinctExprEnumerable<T, TItem> _parent;
            
            public DistinctExprEnumerator Init(
                DistinctExprEnumerable<T, TItem> parent,
                IPoolingEnumerator<T> src,
                Func<T, TItem> selector,
                IEqualityComparer<TItem> comparer)
            {
                _src = src;
                _parent = parent;
                _selector = selector;
                _hashset = Pool<PoolingDictionary<TItem, int>>.Get().Init(0, comparer ?? EqualityComparer<TItem>.Default);
                return this;
            }

            public bool MoveNext()
            {
                while (_src.MoveNext())
                {
                    var key = _selector(_src.Current);
                    if(_hashset.ContainsKey(key)) continue;

                    _hashset[key] = 1;
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
                _selector = default;
                Pool<DistinctExprEnumerator>.Return(this);
            }
        }
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}