namespace MemoryPools.Collections.Linq
{
    internal class OfTypeClauseEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        
        private IPoolingEnumerable _src;

        public OfTypeClauseEnumerable<T> Init(IPoolingEnumerable src)
        {
            _src = src;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<OfTypeClauseEnumerator>().Init(_src.GetEnumerator(), this);
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

        internal class OfTypeClauseEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator _src;
            private OfTypeClauseEnumerable<T> _parent;
        	
            public OfTypeClauseEnumerator Init(IPoolingEnumerator src, OfTypeClauseEnumerable<T> parent)
            {
                _src = src;
                _parent = parent;
                return this;
            }

            public bool MoveNext()
            {
                do
                {
                    var next = _src.MoveNext();
                    if (!next) return false;
                } while (!(_src.Current is T));

                return true;
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