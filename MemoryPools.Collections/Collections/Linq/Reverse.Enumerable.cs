using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    internal class ReverseExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        private PoolingList<T> _src;

        public ReverseExprEnumerable<T> Init(PoolingList<T> src)
        {
            _src = src;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool<ReverseExprEnumerator>.Get().Init(_src, this);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src?.Dispose();
                Pool<PoolingList<T>>.Return(_src);
                _src = default;
                Pool<ReverseExprEnumerable<T>>.Return(this);
            }
        }

        internal class ReverseExprEnumerator : IPoolingEnumerator<T>
        {
            private PoolingList<T> _src;
            private ReverseExprEnumerable<T> _parent;
            private int _position;
        	
            public ReverseExprEnumerator Init(PoolingList<T> src, ReverseExprEnumerable<T> parent)
            {
                _position = src.Count;
                _src = src;
                _parent = parent;
                return this;
            }

            public bool MoveNext()
            {
                if (_position == 0) return false;
                _position--;
                return true;
            }

            public void Reset() => _position = _src.Count;

            object IPoolingEnumerator.Current => Current;

            public T Current => _src[_position];

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = default;
                _src = default;
                _position = default;
                
                Pool<ReverseExprEnumerator>.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}