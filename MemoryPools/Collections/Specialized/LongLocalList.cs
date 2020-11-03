using System;
using System.Collections.Generic;

namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// 	Contains 8 elements as struct fields as Maximum. Use it when you have guaranteed count of elements <= 8
	/// </summary>
	public struct LongLocalList<T>
	{
		private static readonly EqualityComparer<T> ItemComparer = EqualityComparer<T>.Default;

		private (T, T, T, T, T, T, T, T) _items;

		public const int Capacity = 8;

		public void Add(T item)
		{
			Count++;
			if (Count >= Capacity) throw new ArgumentOutOfRangeException();

			this[Count - 1] = item;
		}

		public void Clear()
		{
			Count = 0;
			_items = default;
		}

		public bool Contains(T item)
		{
			return IndexOf(item) >= 0;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			for (var i = 0; i < Capacity; i++) array[i] = this[i];
		}

		public bool Remove(T item)
		{
			var i = 0;
			var j = 0;

			for (; i < Capacity; i++)
			{
				if (ItemComparer.Equals(this[i], item) != true) j++;
				else continue;
				this[j] = this[i];
			}

			return j != i;
		}

		public int Count { get; private set; }

		public bool IsReadOnly => false;

		public int IndexOf(T item)
		{
			for (var i = 0; i < Capacity; i++)
				if (ItemComparer.Equals(this[i], item))
					return i;
			return -1;
		}

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new IndexOutOfRangeException();

				switch (index)
				{
					case 0: return _items.Item1;
					case 1: return _items.Item2;
					case 2: return _items.Item3;
					case 3: return _items.Item4;
					case 4: return _items.Item5;
					case 5: return _items.Item6;
					case 6: return _items.Item7;
					case 7: return _items.Item8;
					default: throw new IndexOutOfRangeException();
				}
			}
			set
			{
				if (index < 0 || index >= Count)
					throw new IndexOutOfRangeException();

				switch (index)
				{
					case 0:
						_items.Item1 = value;
						break;
					case 1:
						_items.Item2 = value;
						break;
					case 2:
						_items.Item3 = value;
						break;
					case 3:
						_items.Item4 = value;
						break;
					case 4:
						_items.Item5 = value;
						break;
					case 5:
						_items.Item6 = value;
						break;
					case 6:
						_items.Item7 = value;
						break;
					case 7:
						_items.Item8 = value;
						break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

		public override int GetHashCode()
		{
			return new Hasher {_items};
		}

		public override bool Equals(object obj)
		{
			return obj is LongLocalList<T> other && Equals(other);
		}

		public bool Equals(LongLocalList<T> other)
		{
			return _items.Equals(other._items);
		}
	}
}