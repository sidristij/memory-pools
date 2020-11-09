using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    internal class IntersectExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        private IPoolingEnumerable<T> _src;
        private IEqualityComparer<T> _comparer;
        private PoolingDictionary<T, int> _second;

        public IntersectExprEnumerable<T> Init(
            IPoolingEnumerable<T> src,
            PoolingDictionary<T, int> second,
            IEqualityComparer<T> comparer = default)
        {
            _src = src;
            _count = 0;
            _second = second;
            _comparer = comparer ?? EqualityComparer<T>.Default;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<IntersectExprEnumerator>().Init(this, _src.GetEnumerator(), _comparer);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                _second?.Dispose();
                Pool.Return(_second);

                _second = default;
                Pool.Return(this);
            }
        }
        
        internal class IntersectExprEnumerator : IPoolingEnumerator<T>
        {
            private IntersectExprEnumerable<T> _parent;
            private IPoolingEnumerator<T> _src;
            private PoolingDictionary<T, int> _alreadyDoneItems;
            
            public IntersectExprEnumerator Init(IntersectExprEnumerable<T> parent, IPoolingEnumerator<T> src, IEqualityComparer<T> comparer)
            {
                _src = src;
                _parent = parent;
                _alreadyDoneItems = Pool.Get<PoolingDictionary<T, int>>().Init(0, comparer);
                return this;
            }

            public bool MoveNext()
            {
                while (_src.MoveNext())
                {
                    if (_parent._second.ContainsKey(_src.Current) && 
                        !_alreadyDoneItems.ContainsKey(_src.Current))
                    {
                        _alreadyDoneItems[_src.Current] = 1;
                        return true;
                    }
                }

                return false;
            }

            public void Reset() => _src.Reset();

            object IPoolingEnumerator.Current => Current;

            public T Current => _src.Current;

            public void Dispose()
            {
                _src?.Dispose();
                _src = null;
                
                _alreadyDoneItems?.Dispose();
                Pool.Return(_alreadyDoneItems);
                _alreadyDoneItems = default;
                
                _parent?.Dispose();
                _parent = default;
                
                Pool.Return(this);
            }
        }
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}