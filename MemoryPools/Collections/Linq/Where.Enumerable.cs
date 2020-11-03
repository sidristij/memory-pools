using System;

namespace MemoryPools.Collections.Linq
{
	internal class WhereClauseEnumerable<T> : IPoolingEnumerable<T>
    {
    	private IPoolingEnumerable<T> _src;
    	private Func<T, bool> _condition;

    	public WhereClauseEnumerable<T> Init(IPoolingEnumerable<T> src, Func<T, bool> condition)
    	{
    		_src = src;
    		_condition = condition;
    		return this;
    	}

    	public IPoolingEnumerator<T> GetEnumerator()
    	{
    		var (condition, src) = (_condition, _src);
    		(_condition, _src) = (default, default);
    		Heap.Return(this);
    		return Heap.Get<WhereClauseEnumerator>().Init(src.GetEnumerator(), condition);
    	}

    	internal class WhereClauseEnumerator : IPoolingEnumerator<T>
    	{
    		private Func<T, bool> _mutator;
    		private IPoolingEnumerator<T> _src;
    
    		public WhereClauseEnumerator Init(IPoolingEnumerator<T> src, Func<T, bool> mutator) 
    		{
    			_src = src;
                _mutator = mutator;
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

    		public T Current => _src.Current;
    
    		public void Dispose()
    		{
    			_src.Dispose();
    			_src = default;
    			Heap.Return(this);
    		}
    	}
    }
}