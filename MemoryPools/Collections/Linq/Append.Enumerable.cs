using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    internal class AppendExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
	    
        private IPoolingEnumerable<T> _src;
        private T _element;

        public AppendExprEnumerable<T> Init(IPoolingEnumerable<T> src, T element)
        {
            _src = src;
            _count = 0;
            _element = element;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return ObjectsPool<AppendExprEnumerator>.Get().Init(_src.GetEnumerator(), this, _element);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                _element = default;
                ObjectsPool<AppendExprEnumerable<T>>.Return(this);
            }
        }

        internal class AppendExprEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator _src;
            private AppendExprEnumerable<T> _parent;
            private T _element;
            private int _overcount;
		    
            public AppendExprEnumerator Init(IPoolingEnumerator src, AppendExprEnumerable<T> parent, T element)
            {
                _src = src;
                _parent = parent;
                _element = element;
                _overcount = 0;
                return this;
            }

            public bool MoveNext()
            {
                if (!_src.MoveNext())
                {
                    if (_overcount == 0)
                    {
                        _overcount++;
                        return true;
                    }

                    _overcount++;
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                _overcount = 0;
                _src.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public T Current => _overcount == 1 ? _element : (T) _src.Current;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = null;
                _src?.Dispose();
                _src = default;
                ObjectsPool<AppendExprEnumerator>.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}