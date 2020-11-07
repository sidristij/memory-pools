using System;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<int> Range(int startIndex, int count)
        {
            return Pool.Get<RangeExprEnumerable>().Init(startIndex, startIndex + count);
        }

        public static IPoolingEnumerable<int> Range(int count)
        {
            return Pool.Get<RangeExprEnumerable>().Init(0, count - 1);
        }
    }

    internal class RangeExprEnumerable : IPoolingEnumerable<int>
    	{
    		private int _start;
    		private int _last;
    		private int _count;
    
    		public RangeExprEnumerable Init(int start, int last)
            {
	            _start = start;
	            _last = last;
	            _count = 0;
    			return this;
    		}
    
    		public IPoolingEnumerator<int> GetEnumerator()
    		{
    			_count++;
    			return Pool.Get<RangeExprEnumerator>().Init(this, _start, _last);
    		}
    
    		private void Dispose()
    		{
    			if (_count == 0) return;
    			_count--;
    			if (_count == 0)
                {
	                _start = _last = 0;
    				_count = 0;
    				Pool.Return(this);
    			}
    		}
    
    		internal class RangeExprEnumerator : IPoolingEnumerator<int>
            {
	            private int _start;
	            private int _current;
	            private int _last;
	            private RangeExprEnumerable _parent;
	            
    			public RangeExprEnumerator Init(RangeExprEnumerable parent, int start, int last)
                {
	                _current = -1;
	                _start = start;
	                _last = last;
	                _parent = parent;
    				return this;
    			}

                public bool MoveNext()
                {
	                if (_current == _last) return false;
	                if (_current == -1)
	                {
		                _current = _start;
		                return _start != _last;
	                }

	                _current++;
	                return _start != _last;
                }
    
    			public void Reset() => _current = _start;
    
    			object IPoolingEnumerator.Current => _current;
    
    			public int Current => _current;
    
    			public void Dispose()
                {
	                _current = -1;
	                _parent?.Dispose();
	                _parent = default;
    				Pool.Return(this);
    			}
    		}
    
    		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
    		{
    			return GetEnumerator();
    		}
    	}
}