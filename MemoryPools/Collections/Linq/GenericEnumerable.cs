using System.Collections;
using System.Collections.Generic;
using MemoryPools.Memory;

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
			ObjectsPool<GenericPoolingEnumerable<T>>.Return(this);
			return ObjectsPool<GenericPoolingEnumerator<T>>.Get().Init(enumerator);
		}

		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
		{
			return GetEnumerator();
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
			ObjectsPool<GenericEnumerable<T>>.Return(this);
			return ObjectsPool<GenericEnumerator<T>>.Get().Init(enumerator);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}