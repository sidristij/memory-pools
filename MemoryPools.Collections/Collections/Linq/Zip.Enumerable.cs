namespace MemoryPools.Collections
{
    internal class ZipExprEnumerable<T> : IPoolingEnumerable<(T, T)>
    {
        private IPoolingEnumerable<T> _src, _second;
        private int _count;

        public ZipExprEnumerable<T> Init(IPoolingEnumerable<T> src, IPoolingEnumerable<T> second)
        {
            _src = src;
            _count = 0;
            _second = second;
            return this;
        }

        public IPoolingEnumerator<(T, T)> GetEnumerator()
        {
            _count++;
            return Pool<ZipExprEnumerator>.Get().Init(this, _src.GetEnumerator(), _second.GetEnumerator());
        }

        private void Dispose()
        {
            if (_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                _second = default;
                Pool<ZipExprEnumerable<T>>.Return(this);
            }
        }

        internal class ZipExprEnumerator : IPoolingEnumerator<(T, T)>
        {
            private ZipExprEnumerable<T> _parent;
            private IPoolingEnumerator<T> _src, _second;
            private bool _hasResult;

            public ZipExprEnumerator Init(
                ZipExprEnumerable<T> parent, IPoolingEnumerator<T> src, IPoolingEnumerator<T> second) 
            {
                _parent = parent;
                _src = src;
                _second = second;
                _hasResult = false;
                return this;
            }

            public bool MoveNext()
            {
                _hasResult = _src.MoveNext() && _second.MoveNext();
                return _hasResult;
            }

            public void Reset()
            {
                _src.Reset();
                _second.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public (T, T) Current => _hasResult ? ( _src.Current, _second.Current) : default;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = default;
                _src?.Dispose();
                _src = default;
                _second?.Dispose();
                _second = default;
                Pool<ZipExprEnumerator>.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}