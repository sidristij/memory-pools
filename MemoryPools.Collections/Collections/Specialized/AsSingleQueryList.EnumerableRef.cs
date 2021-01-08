namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList 
	{
		private class EnumerableShared<T> : IPoolingEnumerable<T> where T : class
		{
			private PoolingListCanon<T> _src;
			private int _count;

			public IPoolingEnumerable<T> Init(PoolingListCanon<T> src)
			{
				_src = src;
				_count = 0;
				return this;
			}

			public IPoolingEnumerator<T> GetEnumerator()
			{
				_count++;
				return Pool<EnumeratorRef>.Get().Init(this, _src);
			}

			IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

			private void Dispose()
			{
				if (_count == 0) return;
				_count--;
				if (_count == 0)
				{
					_src?.Dispose();
					_src = default;
					Pool<EnumerableShared<T>>.Return(this);
				}
			}

			private class EnumeratorRef : IPoolingEnumerator<T> 
			{
				private IPoolingEnumerator<T> _enumerator;
				private EnumerableShared<T> _parent;

				public IPoolingEnumerator<T> Init(EnumerableShared<T> parent, IPoolingEnumerable<T> src)
				{
					_parent = parent;
					_enumerator = src.GetEnumerator();
					return this;
				}

				public bool MoveNext() => _enumerator.MoveNext();

				public void Reset() => _enumerator.Reset();

				public T Current => _enumerator.Current;

				object IPoolingEnumerator.Current => Current;

				public void Dispose()
				{
					_enumerator?.Dispose();
					_enumerator = default;
					
					_parent?.Dispose();
					_parent = default;
					
					Pool<EnumeratorRef>.Return(this);
				}
			}
		}
	}
}