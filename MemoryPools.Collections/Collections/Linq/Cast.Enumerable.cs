using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    internal class CastExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
	    
        private IPoolingEnumerable _src;

        public CastExprEnumerable<T> Init(IPoolingEnumerable src)
        {
            _src = src;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return ObjectsPool<CastExprEnumerator>.Get().Init(_src.GetEnumerator(), this);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                ObjectsPool<CastExprEnumerable<T>>.Return(this);
            }
        }

        internal class CastExprEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator _src;
            private CastExprEnumerable<T> _parent;
		    
            public CastExprEnumerator Init(IPoolingEnumerator src, CastExprEnumerable<T> parent)
            {
                _src = src;
                _parent = parent;
                return this;
            }

            public bool MoveNext()
            {
                return _src.MoveNext();
            }

            public void Reset()
            {
                _src.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public T Current => (T)_src.Current;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = null;
                _src?.Dispose();
                _src = default;
                ObjectsPool<CastExprEnumerator>.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}