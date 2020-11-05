using System;

namespace MemoryPools.Collections.Linq
{
	internal class WhereExprEnumerable<T> : IPoolingEnumerable<T>
	{
		private int _count;
    	private IPoolingEnumerable<T> _src;
    	private Func<T, bool> _condition;

    	public WhereExprEnumerable<T> Init(IPoolingEnumerable<T> src, Func<T, bool> condition)
        {
	        _count = 0;
    		_src = src;
    		_condition = condition;
    		return this;
    	}

    	public IPoolingEnumerator<T> GetEnumerator()
        {
	        _count++;
    		return Pool.Get<WhereExprEnumerator>().Init(_src.GetEnumerator(), this, _condition);
    	}

        private void Dispose()
        {
	        if (_count == 0) return;
	        
	        _count--;
	        
	        if (_count == 0)
	        {
			    _src = default;
		        _condition = default;
		        Pool.Return(this);
	        }
        }

    	internal class WhereExprEnumerator : IPoolingEnumerator<T>
    	{
    		private Func<T, bool> _mutator;
    		private IPoolingEnumerator<T> _src;
            private WhereExprEnumerable<T> _parent;
    
    		public WhereExprEnumerator Init(IPoolingEnumerator<T> src, WhereExprEnumerable<T> parent, Func<T, bool> mutator) 
    		{
    			_src = src;
                _mutator = mutator;
                _parent = parent;
    			return this;
    		}
        		
    		public bool MoveNext()
    		{
    			do
    			{
    				var next = _src.MoveNext();
    				if (!next) return false;
    			} while (!_mutator(_src.Current));

    			return true;
    		}
    
    		public void Reset()
    		{
    			_src.Reset();
    		}

            object IPoolingEnumerator.Current => Current;

            public T Current => _src.Current;
    
    		public void Dispose()
    		{
    			_parent.Dispose();
                _parent = default;
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