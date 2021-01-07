using System.Collections.Generic;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    internal class ExceptExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        private IPoolingEnumerable<T> _src;
        private IEqualityComparer<T> _comparer;
        private PoolingDictionary<T, int> _except;

        public ExceptExprEnumerable<T> Init(IPoolingEnumerable<T> src, PoolingDictionary<T, int> except, IEqualityComparer<T> comparer  = default)
        {
            _src = src;
            _except = except;
            _comparer = comparer;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return ObjectsPool<ExceptExprEnumerator>.Get().Init(this, _src.GetEnumerator());
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                _except?.Dispose();
                ObjectsPool<PoolingDictionary<T, int>>.Return(_except);
                _except = default;
                ObjectsPool<ExceptExprEnumerable<T>>.Return(this);
            }
        }
        internal class ExceptExprEnumerator : IPoolingEnumerator<T>
        {
            private ExceptExprEnumerable<T> _parent;
            private IPoolingEnumerator<T> _src;
            
            public ExceptExprEnumerator Init(ExceptExprEnumerable<T> parent, IPoolingEnumerator<T> src)
            {
                _src = src;
                _parent = parent;
                return this;
            }

            public bool MoveNext()
            {
                while (_src.MoveNext())
                {
                    if(_parent._except.ContainsKey(_src.Current)) continue;
                    return true;
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
                
                _parent?.Dispose();
                _parent = default;
                
                ObjectsPool<ExceptExprEnumerator>.Return(this);
            }
        }
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}