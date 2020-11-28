using System;
using System.Collections;
using System.Collections.Generic;

namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	///     Use this class when you need to store 1 or 2 items in class fields and in very rare scenarios
	///     more then 2 items. In super-rare scenarios - to pass it as IList, ICollection or as IEnumerable.
	/// </summary>
	public struct LocalList<T> : IList<T>
	{
		private static readonly EqualityComparer<T> ItemComparer = EqualityComparer<T>.Default;

		private (T, T) _items;
		private IList<T> _other;

		// Static lists to store real length (-1 field in struct)
		private static readonly IList<T> LengthIs1 = new List<T> {default};
		private static readonly IList<T> LengthIs2 = new List<T> {default, default};
		public const int Capacity = 2;

		public IEnumerator<T> GetEnumerator()
		{
			if (_other == null) yield break;
			if (_other.Count <= Capacity)
			{
				yield return _items.Item1;
				if (ReferenceEquals(_other, LengthIs2)) yield return _items.Item2;
				yield break;
			}

			using(var enumerator = _other.GetEnumerator())
			{
				while (enumerator.MoveNext()) yield return enumerator.Current;
			}
		}

		public void Add(T item)
		{
			if (_other == null)
			{
				_items.Item1 = item;
				_other = LengthIs1;
			}
			else if (ReferenceEquals(_other, LengthIs1))
			{
				_items.Item2 = item;
				_other = LengthIs2;
			}
			else
			{
				if (ReferenceEquals(_other, LengthIs2))
				{
					_other = new List<T>(8);
					_other.Add(_items.Item1);
					_items.Item1 = default;

					_other.Add(_items.Item2);
					_items.Item2 = default;
				}

				_other.Add(item);
			}
		}

		public void Clear()
		{
			_other = null;
			_items.Item1 = default;
			_items.Item2 = default;
		}

		public bool Contains(T item)
		{
			return IndexOf(item) >= 0;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			if (_other == null) return;

			if (_other.Count > Capacity)
			{
				_other.CopyTo(array, arrayIndex);
			}
			else
			{
				if (_other.Count > 0) array[arrayIndex++] = _items.Item1;
				if (_other.Count > 1) array[arrayIndex] = _items.Item2;
			}
		}

		public bool Remove(T item)
		{
			if (_other == null) return false;

			if (_other.Count > Capacity)
			{
				var done = _other.Remove(item);

				if (_other.Count == 2)
				{
					_items.Item1 = _other[0];
					_items.Item2 = _other[1];
					_other = LengthIs2;
				}

				return done;
			}

			if (ReferenceEquals(_other, LengthIs1) && ItemComparer.Equals(_items.Item1, item))
			{
				_items.Item1 = default;
				_other = null;
				return true;
			}

			if (ReferenceEquals(_other, LengthIs2))
			{
				var done = false;
				if (ItemComparer.Equals(_items.Item2, item))
				{
					_items.Item2 = default;
					_other = LengthIs1;
					done = true;
				}

				if (ItemComparer.Equals(_items.Item1, item))
				{
					_items.Item1 = _items.Item2;
					_other = ReferenceEquals(_other, LengthIs1) ? null : LengthIs1;
					done = true;
				}

				return done;
			}

			return false;
		}

		public int Count => _other?.Count ?? 0;

		public bool IsReadOnly => false;

		public int IndexOf(T item)
		{
			if (_other == null)
				return -1;

			if (_other.Count > Capacity) return _other.IndexOf(item);

			if (_other.Count > 0 && ItemComparer.Equals(_items.Item1, item)) return 0;
			if (_other.Count > 1 && ItemComparer.Equals(_items.Item2, item)) return 1;

			return -1;
		}

		public void Insert(int index, T item)
		{
			// 2nd of 1, 3rd of 2, 4th of 3..
			if (index == Count)
			{
				Add(item);
				return;
			}

			// Asked non-first when empty
			if (_other == null) throw new IndexOutOfRangeException();

			// If list already created
			if (_other.Count > Capacity) _other.Insert(index, item);

			if (index == 0)
			{
				if (ReferenceEquals(_other, LengthIs1))
				{
					_items = (item, _items.Item1);
					_other = LengthIs2;
				}
				else
				{
					(_items.Item1, _items.Item2, item) = (item, _items.Item1, _items.Item2);
					Add(item);
				}

				return;
			}

			// (item0, item1), list(nothing) ->> (def, def), list(item0, item ,item1)
			if (index == 1)
			{
				(_items.Item2, item) = (item, _items.Item2);
				Add(item);
			}
		}

		public void RemoveAt(int index)
		{
			if (_other == null || _other.Count <= index || index < 0)
				throw new IndexOutOfRangeException();

			if (_other.Count < Capacity)
			{
				if (index == 0)
				{
					_items.Item1 = _items.Item2;
					_other = ReferenceEquals(_other, LengthIs1) ? null : LengthIs1;
				}
				else
				{
					_items.Item2 = default;
					_other = LengthIs1;
				}
			}
			else
			{
				_other.RemoveAt(index);
				if (_other.Count == 2)
				{
					_items.Item1 = _other[0];
					_items.Item2 = _other[1];
					_other = LengthIs2;
				}
			}
		}

		public T this[int index]
		{
			get
			{
				if (_other == null || index >= Count || index < 0)
					throw new IndexOutOfRangeException();

				if (_other?.Count > Capacity) return _other[index];
				if (_other.Count > 0 && index == 0) return _items.Item1;
				if (_other.Count > 1 && index == 1) return _items.Item2;

				throw new InvalidOperationException("Uncovered branch");
			}
			set
			{
				if (_other == null || index >= Count || index < 0)
					throw new IndexOutOfRangeException();

				if (_other.Count > Capacity) _other[index] = value;
				if (_other.Count > 0 && index == 0) _items.Item1 = value;
				if (_other.Count > 1 && index == 1) _items.Item2 = value;

				throw new InvalidOperationException("Uncovered branch");
			}
		}

		public override int GetHashCode()
		{
			return new Hasher {_items, _other};
		}

		public override bool Equals(object obj)
		{
			return obj is LocalList<T> other && Equals(other);
		}

		public bool Equals(LocalList<T> other)
		{
			return _items.Equals(other._items) && Equals(_other, other._other);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}