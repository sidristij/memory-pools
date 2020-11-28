using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace MemoryPools.Memory
{
	/// <summary>
	///     Encapsulates manual memory management mechanism. Holds
	///     IMemoryOwner instance goes to GC (link = null) only when
	///     all owning entities called Dispose() method. This means,
	///     that only this mechanism should be used for covering
	///     managed instances.
	/// </summary>
	public sealed class CountdownMemoryOwner<T> : IMemoryOwner<T>
	{
		private int _length;
		private int _offset;
		private int _owners;
		private T[] _arr;
		private CountdownMemoryOwner<T> _parent;
		private Memory<T> _memory;

		public Memory<T> Memory
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _memory;

			private set => _memory = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal CountdownMemoryOwner<T> Init(CountdownMemoryOwner<T> parent, int offset, int length, bool defaultOwner = true)
		{
			_owners = defaultOwner?1:0;
			_offset = offset;
			_length = length;
			_parent = parent;
			_parent.AddOwner();
			Memory = _parent.Memory.Slice(_offset, _length);
			return this;
		}
        
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal CountdownMemoryOwner<T> Init(T[] array, int length)
		{
			_owners = 1;
			_offset = 0;
			_length = length;
			_parent = default;
			_arr = array;
			Memory = _arr.AsMemory(0, _length);
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddOwner() => _owners++;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			_owners--;
			if (_owners > 0)
			{
				return;
			}

			if (_parent != default)
			{
				_parent.Dispose();
				_parent = null;
			}
			else
			{
				ArrayPool<T>.Shared.Return(_arr);
			}
			ObjectsPool<CountdownMemoryOwner<T>>.Return(this);
		}
	}
}