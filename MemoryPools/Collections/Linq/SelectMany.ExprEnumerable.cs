using System;

namespace MemoryPools.Collections.Linq
{
    internal class SelectManyExprEnumerable<T, TR> : IPoolingEnumerable<TR>
    {
        private IPoolingEnumerable<T> _src;
        private Func<T, IPoolingEnumerable<TR>> _mutator;
        private int _count;

        public SelectManyExprEnumerable<T, TR> Init(IPoolingEnumerable<T> src, Func<T, IPoolingEnumerable<TR>> mutator)
        {
        	_src = src;
        	_count = 0;
        	_mutator = mutator;
        	return this;
        }

        public IPoolingEnumerator<TR> GetEnumerator()
        {
        	_count++;
        	return Pool.Get<SelectManyExprEnumerator>().Init(this, _src.GetEnumerator(), _mutator);
        }

        private void Dispose()
        {
        	if (_count == 0) return;
        	_count--;
        	if (_count == 0)
        	{
        		_src = default;
        		_count = 0;
        		_mutator = default;
        		Pool.Return(this);
        	}
        }

        internal class SelectManyExprEnumerator : IPoolingEnumerator<TR>
        {
        	private Func<T, IPoolingEnumerable<TR>> _mutator;
        	private SelectManyExprEnumerable<T, TR> _parent;
        	private IPoolingEnumerator<T> _src;
            private IPoolingEnumerator<TR> _currentEnumerator;
            private bool _finished;
            
        	public SelectManyExprEnumerator Init(
	            SelectManyExprEnumerable<T, TR> parent,
	            IPoolingEnumerator<T> src,
	            Func<T, IPoolingEnumerable<TR>> mutator) 
        	{
        		_src = src;
                _finished = false;
        		_parent = parent;
        		_mutator = mutator;
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
					_currentEnumerator = _mutator(_src.Current).GetEnumerator();
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
		            _currentEnumerator = _mutator(_src.Current).GetEnumerator();
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
        		
                Pool.Return(this);
        	}
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
        {
        	return GetEnumerator();
        }
    }
}