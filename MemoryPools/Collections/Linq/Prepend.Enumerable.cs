namespace MemoryPools.Collections.Linq
{
    internal class PrependExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
	    
        private IPoolingEnumerable<T> _src;
        private T _element;

        public PrependExprEnumerable<T> Init(IPoolingEnumerable<T> src, T element)
        {
            _src = src;
            _count = 0;
            _element = element;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<PrependExprEnumerator>().Init(_src.GetEnumerator(), this, _element);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                _element = default;
                Pool.Return(this);
            }
        }

        internal class PrependExprEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator _src;
            private PrependExprEnumerable<T> _parent;
            private T _element;
            private bool _first, _shouldReturnElement;
		    
            public PrependExprEnumerator Init(IPoolingEnumerator src, PrependExprEnumerable<T> parent, T element)
            {
                _src = src;
                _parent = parent;
                _element = element;
                _first = true;
                _shouldReturnElement = true;
                return this;
            }

            public bool MoveNext()
            {
                if (_first)
                {
                    _first = false;
                    return true;
                }

                _shouldReturnElement = false;
                return _src.MoveNext();
            }

            public void Reset()
            {
                _first = true;
                _src.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public T Current => _shouldReturnElement ? _element : (T) _src.Current;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = null;
                _src?.Dispose();
                _src = default;
                _first = _shouldReturnElement = false;
                Pool.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}