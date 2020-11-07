using System;
using System.Buffers;

namespace MemoryPools.Collections.Specialized
{
	public abstract class PoolingListBase<T> : IDisposable, IPoolingEnumerable<T>
	{
		protected IMemoryOwner<IPoolingNode<T>> _root;
		protected int _count;
		protected int _ver;

		public IPoolingEnumerator<T> GetEnumerator()
		{
			return Pool.Get<Enumerator>().Init(this);
		}

		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected abstract IPoolingNode<T> CreateNodeHolder();
		
		public void Add(T item)
		{
			var bn = _count >> PoolsDefaults.DefaultPoolBucketDegree;
			var bi = _count & PoolsDefaults.DefaultPoolBucketMask;

			_root.Memory.Span[bn] ??= CreateNodeHolder();

			_root.Memory.Span[bn][bi] = item;

			_count++;

			unchecked
			{
				_ver++;
			}
		}

		public void Clear()
		{
			for (int i = 0, len = _root.Memory.Span.Length; i < len; i++)
			{
				if (_root.Memory.Span[i] == null) break;
				_root.Memory.Span[i].Clear();
				_root.Memory.Span[i].Dispose();
				_root.Memory.Span[i] = default;
			}

			_count = default;

			unchecked
			{
				_ver++;
			}
		}
		
		public bool Contains(T item) => IndexOf(item) != -1;

		public void CopyTo(T[] array, int arrayIndex)
		{
			var len = 0;
			for (var i = 0; i <= PoolsDefaults.DefaultPoolBucketSize; i++)
			for (var j = 0; j < PoolsDefaults.DefaultPoolBucketSize && len < _count; j++, len++)
			{
				array[len] = _root.Memory.Span[i][j];
			}
		}

		public bool Remove(T item)
		{
			int i, j;
			for (i = 0, j = 0; i < _count; i++)
			{
				var bfn = i >> PoolsDefaults.DefaultPoolBucketDegree;
				var bfi = i & PoolsDefaults.DefaultPoolBucketMask;
				var btn = j >> PoolsDefaults.DefaultPoolBucketDegree;
				var bti = j & PoolsDefaults.DefaultPoolBucketMask;

				if (!_root.Memory.Span[bfn][bfi].Equals(item))
				{
					_root.Memory.Span[btn][bti] = _root.Memory.Span[bfn][bfi];
					j++;
				}
				else
				{
					_count--;
				}
			}

			unchecked
			{
				_ver++;
			}

			return i != j && i != 0;
		}

		public int Count => _count;

		public bool IsReadOnly => false;

		public int IndexOf(T item)
		{
			var len = 0;
			
			for (var i = 0; i <= PoolsDefaults.DefaultPoolBucketSize; i++)
			for (var j = 0; j < PoolsDefaults.DefaultPoolBucketSize && len < _count; j++, len++)
			{
				if (item.Equals(_root.Memory.Span[i][j])) return len;
			}

			return -1;
		}

		public void Insert(int index, T item)
		{
			if (index < _count)
			{
				throw new IndexOutOfRangeException(nameof(index));
			}

			for (var i = index; i <= _count; i++)
			{
				var j = i + 1;

				var bn = i >> PoolsDefaults.DefaultPoolBucketDegree;
				var bi = i & PoolsDefaults.DefaultPoolBucketMask;

				var bjn = j >> PoolsDefaults.DefaultPoolBucketDegree;
				var bji = j & PoolsDefaults.DefaultPoolBucketMask;

				var copy = _root.Memory.Span[bn][bi];
				_root.Memory.Span[bjn][bji] = item;
				item = copy;
			}

			_count++;
			unchecked
			{
				_ver++;
			}
		}

		public void RemoveAt(int index)
		{
			if (index >= _count)
			{
				throw new IndexOutOfRangeException(nameof(index));
			}
			
			for (int i = index, j = i + 1; i <= _count; i++)
			{
				var bn = i >> PoolsDefaults.DefaultPoolBucketDegree;
				var bi = i & PoolsDefaults.DefaultPoolBucketMask;

				var bjn = j >> PoolsDefaults.DefaultPoolBucketDegree;
				var bji = j & PoolsDefaults.DefaultPoolBucketMask;

				_root.Memory.Span[bn][bi] = _root.Memory.Span[bjn][bji];
			}

			_count--;
			unchecked
			{
				_ver++;
			}
		}

		public void Resize(int size)
		{
			if (size == _count) return;
			if (size < _count)
			{
				var cbn = _count >> PoolsDefaults.DefaultPoolBucketDegree;
				var sbn = size >> PoolsDefaults.DefaultPoolBucketDegree;
				var sbi = size & PoolsDefaults.DefaultPoolBucketMask;

				for (var bn = sbn + 1; bn <= cbn; bn++)
				{
					_root.Memory.Span[bn].Dispose();
					_root.Memory.Span[bn] = default;
				}

				var span = _root.Memory.Span[sbn];
				for (var i = sbi; i <= PoolsDefaults.DefaultPoolBucketSize; i++)
				{
					span[i] = default;
				}

				_count = size;
			}
			else
			{
				var cbn = _count >> PoolsDefaults.DefaultPoolBucketDegree;
				var sbn = size >> PoolsDefaults.DefaultPoolBucketDegree;
				
				for (var bn = cbn + 1; bn <= sbn; bn++)
				{
					_root.Memory.Span[bn] = CreateNodeHolder();
				}

				_count = size;
			}
		}

		public T this[int index]
		{
			get
			{
				if (index >= _count)
				{
					throw new IndexOutOfRangeException(nameof(index));
				}

				var bn = index >> PoolsDefaults.DefaultPoolBucketDegree;
				var bi = index & PoolsDefaults.DefaultPoolBucketMask;
				return _root.Memory.Span[bn][bi];
			}
			set
			{
				if (index >= _count)
				{
					throw new IndexOutOfRangeException(nameof(index));
				}
				
				var bn = index >> PoolsDefaults.DefaultPoolBucketDegree;
				var bi = index & PoolsDefaults.DefaultPoolBucketMask;
				_root.Memory.Span[bn][bi] = value;

				unchecked
				{
					_ver++;
				}
			}
		}

		public void Dispose()
		{
			Clear();
			_root?.Dispose();
			_root = default;
		}

		private class Enumerator : IPoolingEnumerator<T>
		{
			private PoolingListBase<T> _src;
			private int _bucket, _index, _ver;

			public Enumerator Init(PoolingListBase<T> src)
			{
				_bucket = 0;
				_index = -1;
				_src = src;
				_ver = _src._ver;
				return this;
			}
			
			public bool MoveNext()
			{
				if (_index >= _src.Count) return false;
				if (_ver != _src._ver)
				{
					throw new InvalidOperationException("Collection was changed while enumeration");
				}
				
				_index++;
				var tb = _src._count >> PoolsDefaults.DefaultPoolBucketDegree;
				var ti = _src._count & PoolsDefaults.DefaultPoolBucketMask;

				if (_index == PoolsDefaults.DefaultPoolBucketSize)
				{
					_index = 0;
					_bucket++;
				}
				
				if ((_bucket < tb && _index < PoolsDefaults.DefaultPoolBucketSize) || 
				    (_bucket == tb && _index < ti))
				{
					return true;
				}

				return false;
			}

			public void Reset()
			{
				_index = -1;
				_bucket = 0;
				_ver = _src._ver;
			}

			public T Current
			{
				get
				{
					if (_ver != _src._ver)
					{
						throw new InvalidOperationException("Collection was changed while enumeration");
					}
					return _src._root.Memory.Span[_bucket][_index];
				}
			}

			object IPoolingEnumerator.Current => Current;

			public void Dispose()
			{
				Pool.Return(this);
			}
		}
	}
}