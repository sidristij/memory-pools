using System.Collections;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
	internal sealed class GenericPoolingEnumerator<T> : IPoolingEnumerator<T>
	{
		private IEnumerator<T> _source;
    
		public GenericPoolingEnumerator<T> Init(IEnumerator<T> source)
		{
			_source = source;
			return this;
		}

		public bool MoveNext() => _source.MoveNext();

		public void Reset() => _source.Reset();
		
		object IPoolingEnumerator.Current => Current;

		public T Current => _source.Current;
    
		public void Dispose()
		{
			_source.Dispose();
			_source = default;
			Pool<GenericPoolingEnumerator<T>>.Return(this);
		}
	}

	internal sealed class GenericEnumerator<T> : IEnumerator<T>
	{
		private IPoolingEnumerator<T> _source;
    
		public GenericEnumerator<T> Init(IPoolingEnumerator<T> source)
		{
			_source = source;
			return this;
		}

		public bool MoveNext() => _source.MoveNext();

		public void Reset() => _source.Reset();
		
		object IEnumerator.Current => Current;

		public T Current => _source.Current;
    
		public void Dispose()
		{
			_source.Dispose();
			_source = default;
			Pool<GenericEnumerator<T>>.Return(this);
		}
	}
}