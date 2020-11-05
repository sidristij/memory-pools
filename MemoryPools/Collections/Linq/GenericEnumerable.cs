using System.Collections;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
	internal class GenericPoolingEnumerable<T> : IPoolingEnumerable<T>
	{
		private IEnumerable<T> _enumerable;
    
		public GenericPoolingEnumerable<T> Init(IEnumerable<T> enumerable)
		{
			_enumerable = enumerable;
			return this;
		}
    			
		public IPoolingEnumerator<T> GetEnumerator()
		{
			var enumerator = _enumerable.GetEnumerator();
			_enumerable = default;
			Pool.Return(this);
			return Pool.Get<GenericPoolingEnumerator<T>>().Init(enumerator);
		}
	}
	internal class GenericEnumerable<T> : IEnumerable<T>
	{
		private IPoolingEnumerable<T> _enumerable;
    
		public GenericEnumerable<T> Init(IPoolingEnumerable<T> enumerable)
		{
			_enumerable = enumerable;
			return this;
		}
    			
		public IEnumerator<T> GetEnumerator()
		{
			var enumerator = _enumerable.GetEnumerator();
			_enumerable = default;
			Pool.Return(this);
			return Pool.Get<GenericEnumerator<T>>().Init(enumerator);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}