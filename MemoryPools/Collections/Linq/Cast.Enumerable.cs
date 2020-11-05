namespace MemoryPools.Collections.Linq
{
    internal class CastClauseEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
	    
        private IPoolingEnumerable _src;

        public CastClauseEnumerable<T> Init(IPoolingEnumerable src)
        {
            _src = src;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<CastClauseEnumerator>().Init(_src.GetEnumerator(), this);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                Pool.Return(this);
            }
        }

        internal class CastClauseEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator _src;
            private CastClauseEnumerable<T> _parent;
		    
            public CastClauseEnumerator Init(IPoolingEnumerator src, CastClauseEnumerable<T> parent)
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
                Pool.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}