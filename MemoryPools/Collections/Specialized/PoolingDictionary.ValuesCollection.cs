using System;
using System.Collections;
using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public partial class PoolingDictionary<TKey, TValue>
	{
		internal class ValuesCollection : ICollection<TValue>
		{
			private PoolingDictionary<TKey, TValue> _src;

			public ValuesCollection Init(PoolingDictionary<TKey, TValue> src)
			{
				_src = src;
				return this;
			}

			public IEnumerator<TValue> GetEnumerator()
			{
				return Heap.Get<Enumerator>().Init(_src);
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			public void Add(TValue item) => throw new NotImplementedException();

			public void Clear() => throw new NotImplementedException();

			public bool Contains(TValue item)
			{
				foreach (var entry in _src._entries)
				{
					if (entry.Equals(item)) return true;
				}

				return false;
			}

			public void CopyTo(TValue[] array, int arrayIndex)
			{
				if (array.Length - arrayIndex < _src._entries.Count)
				{
					throw new IndexOutOfRangeException("Cannot copy keys into array. Target array is too small.");
				}

				for (var index = 0; index < _src._entries.Count; index++)
				{
					var entry = _src._entries[index];
					array[arrayIndex + index] = entry.value;
				}
			}

			public bool Remove(TValue item) => throw new NotImplementedException("Removal ops are disallowed.");
			public int Count => _src.Count;
			public bool IsReadOnly => _src.IsReadOnly;

			private class Enumerator : IEnumerator<TValue>
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

				public TValue Current => _src._entries[_pos].value;

				object IEnumerator.Current => Current;

				public void Dispose()
				{
					_src = default;
					Heap.Return(this);
				}
			}
		}
	}
}