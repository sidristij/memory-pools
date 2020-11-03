using System.Collections;
using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList
	{
		private class EnumerableVal<T> : IEnumerable<T> where T : struct
		{
			private PoolingListVal<T> _src;

			public IEnumerable<T> Init(PoolingListVal<T> src)
			{
				_src = src;
				return this;
			}

			public IEnumerator<T> GetEnumerator()
			{
				var src = _src;
				_src = default;
				Heap.Return(this);
				return Heap.Get<EnumeratorVal>().Init(src);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
			
			private class EnumeratorVal : IEnumerator<T>
			{
				private PoolingListVal<T> _src;
				private IEnumerator<T> _enumerator;

				public IEnumerator<T> Init(PoolingListVal<T> src)
				{
					_src = src;
					_enumerator = _src.GetEnumerator();
					return this;
				}

				public bool MoveNext() => _enumerator.MoveNext();

				public void Reset()
				{
					_enumerator?.Dispose();
					_enumerator = _src.GetEnumerator();
				}

				public T Current => _enumerator.Current;

				object IEnumerator.Current => Current;

				public void Dispose()
				{
					_enumerator?.Dispose();
					_src?.Dispose();
					Heap.Return(_src);
					Heap.Return(this);
					_src = default;
				}
			}
		}
	}
}