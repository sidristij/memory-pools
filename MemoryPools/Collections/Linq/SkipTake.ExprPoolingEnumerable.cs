namespace MemoryPools.Collections.Linq
{
    internal sealed class SkipTakeExprPoolingEnumerable<T> : IPoolingEnumerable<T>
    {
        private bool _take;
        private int _workCount;
        private int _count;
        private IPoolingEnumerable<T> _source;
        
        public SkipTakeExprPoolingEnumerable<T> Init(IPoolingEnumerable<T> source, bool take, int count)
        {
            _count = 0;
            _workCount = count;
            _source = source;
            _take = take;
            return this;
        }
        
        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool.Get<SkipTakeExprPoolingEnumerator>().Init(this, _source.GetEnumerator(), _take, _workCount);
        }

        private void Dispose()
        {
            if (_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _source = null;
                _take = default;
                Pool.Return(this);
            }
        }
       
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
        internal sealed class SkipTakeExprPoolingEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator<T> _source;
            private SkipTakeExprPoolingEnumerable<T> _parent;
            private bool _take;
            private int _pos, _workCount;
        
            public SkipTakeExprPoolingEnumerator Init(SkipTakeExprPoolingEnumerable<T> parent, IPoolingEnumerator<T> source, bool take, int workCount)
            {
                _pos = 0;
                _take = take;
                _source = source;
                _parent = parent;
                _workCount = workCount;
                return this;
            }

            public bool MoveNext()
            {
                if (_take)
                {
                    if (_pos < _workCount)
                    {
                        _pos++;
                        return _source.MoveNext();
                    }

                    return false;
                }

                while (_pos < _workCount)
                {
                    _pos++;
                    _source.MoveNext();
                }

                return _source.MoveNext();
            }

            public void Reset()
            {
                _pos = 0;
                _source.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public T Current => _source.Current;
        
            public void Dispose()
            {
                _parent?.Dispose();
                _parent = default;
                
                _source?.Dispose();
                _source = default;
                
                Pool.Return(this);
            }
        }
    }
}