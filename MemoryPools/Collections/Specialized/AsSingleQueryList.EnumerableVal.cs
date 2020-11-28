using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList
	{
		private class EnumerableTyped<T> : IPoolingEnumerable<T>
		{
			private PoolingList<T> _src;

			public IPoolingEnumerable<T> Init(PoolingList<T> src)
			{
				_src = src;
				return this;
			}

			public IPoolingEnumerator<T> GetEnumerator()
			{
				var src = _src;
				_src = default;
				ObjectsPool<EnumerableTyped<T>>.Return(this);
				return ObjectsPool<EnumeratorVal>.Get().Init(src);
			}

			IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

			private class EnumeratorVal : IPoolingEnumerator<T>
			{
				private PoolingList<T> _src;
				private IPoolingEnumerator<T> _enumerator;

				public IPoolingEnumerator<T> Init(PoolingList<T> src)
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

				object IPoolingEnumerator.Current => Current;

				public void Dispose()
				{
					_enumerator?.Dispose();
					_src?.Dispose();
					ObjectsPool<PoolingList<T>>.Return(_src);
					ObjectsPool<EnumeratorVal>.Return(this);
					_src = default;
				}
			}
		}
	}
}