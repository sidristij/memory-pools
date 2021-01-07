using System;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    internal class SelectManyExprWithContextEnumerable<T, TR, TContext> : IPoolingEnumerable<TR>
    {
        private IPoolingEnumerable<T> _src;
        private Func<T, TContext, IPoolingEnumerable<TR>> _mutator;
        private int _count;
        private TContext _context;

        public SelectManyExprWithContextEnumerable<T, TR, TContext> Init(
            IPoolingEnumerable<T> src, 
            Func<T, TContext, IPoolingEnumerable<TR>> mutator,
            TContext context)
        {
            _src = src;
            _count = 0;
            _context = context;
            _mutator = mutator;
            return this;
        }

        public IPoolingEnumerator<TR> GetEnumerator()
        {
            _count++;
            return ObjectsPool<SelectManyExprWithContextEnumerator>.Get().Init(this, _src.GetEnumerator(), _mutator, _context);
        }

        private void Dispose()
        {
            if (_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                _count = 0;
                _context = default;
                _mutator = default;
                ObjectsPool<SelectManyExprWithContextEnumerable<T, TR, TContext>>.Return(this);
            }
        }

        internal class SelectManyExprWithContextEnumerator : IPoolingEnumerator<TR>
        {
            private TContext _context;
            private Func<T, TContext, IPoolingEnumerable<TR>> _mutator;
            private SelectManyExprWithContextEnumerable<T, TR, TContext> _parent;
            private IPoolingEnumerator<T> _src;
            private IPoolingEnumerator<TR> _currentEnumerator;
            private bool _finished;
            
            public SelectManyExprWithContextEnumerator Init(
                SelectManyExprWithContextEnumerable<T, TR, TContext> parent,
                IPoolingEnumerator<T> src,
                Func<T, TContext, IPoolingEnumerable<TR>> mutator,
                TContext context) 
            {
                _src = src;
                _finished = false;
                _parent = parent;
                _mutator = mutator;
                _context = context;
                _currentEnumerator = default;
                return this;
            }

            public bool MoveNext()
            {
                if (_finished) return false;
                if (_currentEnumerator == default)
                {
                    if (!_src.MoveNext())
                    {
                        _finished = true;
                        return false;
                    }
                    _currentEnumerator = _mutator(_src.Current, _context).GetEnumerator();
                }

                do
                {
                    var hasValue = _currentEnumerator.MoveNext();
                    if (hasValue) return true;
                    if (!_src.MoveNext())
                    {
                        _finished = true;
                        return false;
                    }
                    _currentEnumerator?.Dispose();
                    _currentEnumerator = _mutator(_src.Current, _context).GetEnumerator();
                } while (true);
            }

            public void Reset()
            {
                _currentEnumerator?.Dispose();
                _currentEnumerator = default;
                _src.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public TR Current => _currentEnumerator.Current;

            public void Dispose()
            {
                _currentEnumerator?.Dispose();
                _currentEnumerator = default;
	            
                _parent?.Dispose();
                _parent = default;
        		
                _src.Dispose();
                _src = default;
        		
                ObjectsPool<SelectManyExprWithContextEnumerator>.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}