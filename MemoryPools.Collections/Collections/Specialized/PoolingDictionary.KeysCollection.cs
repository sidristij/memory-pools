using System;
using System.Collections;
using System.Collections.Generic;

namespace MemoryPools.Collections.Specialized
{
	public partial class PoolingDictionary<TKey, TValue>
	{
		internal class KeysCollection : ICollection<TKey>
		{
			private PoolingDictionary<TKey, TValue> _src;

			public KeysCollection Init(PoolingDictionary<TKey, TValue> src)
			{
				_src = src;
				return this;
			}

			public IEnumerator<TKey> GetEnumerator()
			{
				return Pool<Enumerator>.Get().Init(_src);
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			public void Add(TKey item) => throw new NotImplementedException();
			public void Clear() => throw new NotImplementedException();
			public bool Contains(TKey item) => _src.FindEntry(item) >= 0;

			public void CopyTo(TKey[] array, int arrayIndex)
			{
				if (array.Length - arrayIndex < _src._entries.Count)
				{
					throw new IndexOutOfRangeException("Cannot copy keys into array. Target array is too small.");
				}

				for (var index = 0; index < _src._entries.Count; index++)
				{
					var entry = _src._entries[index];
					array[arrayIndex + index] = entry.key;
				}
			}

			public bool Remove(TKey item) => throw new NotImplementedException("Removal ops are disallowed.");
			public int Count => _src.Count;
			public bool IsReadOnly => _src.IsReadOnly;

			private class Enumerator : IEnumerator<TKey>
			{
				private PoolingDictionary<TKey, TValue> _src;
				private int _pos;

				public Enumerator Init(PoolingDictionary<TKey, TValue> src)
				{
					_pos = -1;
					_src = src;
					return this;
				}

				public bool MoveNext()
				{
					if (_pos >= _src.Count) return false;
					_pos++;
					return _pos < _src.Count;
				}

				public void Reset()
				{
					_pos = -1;
				}

				public TKey Current => _src._entries[_pos].key;

				object IEnumerator.Current => Current;

				public void Dispose()
				{
					_src = default;
					Pool<Enumerator>.Return(this);
				}
			}
		}
	}
	
	
}