using System;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
		public static IPoolingEnumerable<T> Empty<T>() => Range(0,0).Select(x => (T)(object)x); 

		public static IPoolingEnumerable<int> Range(int startIndex, int count)
        {
            return Pool<RangeExprEnumerable>.Get().Init(startIndex, count);
        }

        public static IPoolingEnumerable<int> Range(int count)
        {
            return Pool<RangeExprEnumerable>.Get().Init(0, count);
        }

        public static IPoolingEnumerable<T> Repeat<T>(T element, int count) => Range(0, count).Select(element, (item, x) => item);

        public static bool Contains<T>(this IPoolingEnumerable<T> self, T element)
        {
	        foreach (var item in self)
	        {
		        if (item.Equals(element)) return true;
	        }

	        return false;
        }

        public static int Count<T>(this IPoolingEnumerable<T> self)
        {
	        var count = 0;
	        foreach (var _ in self)
	        {
		        count++;
	        }
	        return count;
        }

        public static long LongCount<T>(this IPoolingEnumerable<T> self)
        {
	        long count = 0;
	        foreach (var _ in self)
	        {
		        count++;
	        }
	        return count;
        }

        public static T ElementAt<T>(this IPoolingEnumerable<T> self, int position)
        {
	        var i = 0;
	        foreach (var item in self)
	        {
		        if (i == position) return item;
		        i++;
	        }

	        throw new InvalidOperationException("Sequence is too small. Index not found");
        }

        public static bool SequenceEqual<T>(this IPoolingEnumerable<T> self, IPoolingEnumerable<T> other)
        {
	        var comparer = EqualityComparer<T>.Default;
	        using (var left = self.GetEnumerator())
	        using (var right = other.GetEnumerator())
	        {
		        bool equals, leftHas, rightHas;
		        
		        do
		        {
			        leftHas = left.MoveNext();
			        rightHas = right.MoveNext();
			        equals = comparer.Equals(left.Current, right.Current);
			        
			        if (leftHas != rightHas || !equals) return false;
		        } while (leftHas && rightHas);

		        return !leftHas && !rightHas;
	        }
        }
    }

    internal class RangeExprEnumerable : IPoolingEnumerable<int>
    	{
    		private int _start;
    		private int _workCount;
    		private int _count;
    
    		public RangeExprEnumerable Init(int start, int count)
            {
	            _start = start;
	            _workCount = count;
	            _count = 0;
    			return this;
    		}
    
    		public IPoolingEnumerator<int> GetEnumerator()
    		{
    			_count++;
    			return Pool<RangeExprEnumerator>.Get().Init(this, _start, _workCount);
    		}
    
    		private void Dispose()
    		{
    			if (_count == 0) return;
    			_count--;
    			if (_count == 0)
                {
	                _start = _workCount = 0;
    				_count = 0;
    				Pool<RangeExprEnumerable>.Return(this);
    			}
    		}
    
    		internal class RangeExprEnumerator : IPoolingEnumerator<int>
            {
	            private int _start;
	            private int _current;
	            private int _workCount;
	            private RangeExprEnumerable _parent;
	            
    			public RangeExprEnumerator Init(RangeExprEnumerable parent, int start, int workCount)
                {
	                _current = -1;
	                _start = start;
	                _workCount = workCount;
	                _parent = parent;
    				return this;
    			}

                public bool MoveNext()
                {
	                if (_current == 0) return false;
	                if (_current == -1)
	                {
		                _current = _workCount;
		                return _workCount != 0;
	                }

	                _current--;
	                return _current != 0;
                }
    
    			public void Reset() => _current = _start;
    
    			object IPoolingEnumerator.Current => _current;
    
    			public int Current => _start + (_workCount - _current);
    
    			public void Dispose()
                {
	                _current = -1;
	                _parent?.Dispose();
	                _parent = default;
    				Pool<RangeExprEnumerator>.Return(this);
    			}
    		}
    
    		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
    		{
    			return GetEnumerator();
    		}
    	}
}