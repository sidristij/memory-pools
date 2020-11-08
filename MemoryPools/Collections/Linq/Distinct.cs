using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Distinct<T>(this IPoolingEnumerable<T> source)
        {
            var dictionary = Pool.Get<PoolingDictionary<T,bool>>().Init(0);
            foreach (var item in source)
            {
                dictionary[item] = true;
            }
            return Pool.Get<DistinctExprEnumerable<T>>().Init(dictionary);
        }
    }

    internal class DistinctExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        private PoolingDictionary<T, bool> _parent;

        public DistinctExprEnumerable<T> Init(PoolingDictionary<T,bool> parent)
        {
            _parent = parent;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<DistinctExprEnumerator>().Init(_parent.GetEnumerator(), this);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _parent?.Dispose();
                Pool.Return(_parent);
                _parent = default;
                
                Pool.Return(this);
            }
        }

        internal class DistinctExprEnumerator : IPoolingEnumerator<T>
        {
            private IEnumerator<KeyValuePair<T, bool>> _src;
            private DistinctExprEnumerable<T> _parent;
            
            public DistinctExprEnumerator Init(IEnumerator<KeyValuePair<T, bool>> src, DistinctExprEnumerable<T> parent)
            {
                _src = src;
                _parent = parent;
                return this;
            }

            public bool MoveNext() => _src.MoveNext();

            public void Reset() => _src.Reset();

            object IPoolingEnumerator.Current => Current;

            public T Current => _src.Current.Key;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = default;
                
                _src?.Dispose();
                _src = default;
                
                Pool.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}