using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

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
		private IMemoryOwner<T> _parent;

		public Memory<T> Memory => _parent.Memory.Slice(_offset, _length);

		public void Dispose()
		{
			DeleteOwner();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal CountdownMemoryOwner<T> Init(IMemoryOwner<T> parent, int offset, int length, bool noOwner)
		{
			_owners = noOwner ? 0 : 1;
			if (length > parent.Memory.Length)
				throw new ArgumentOutOfRangeException(nameof(length),
					"Passed length is higher than internal buffer length");

			_offset = offset;
			_length = length;
			_parent = parent.AddOwner();

			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IMemoryOwner<T> AddOwner()
		{
			Interlocked.Increment(ref _owners);
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IMemoryOwner<T> DeleteOwner()
		{
			var owners = Interlocked.Decrement(ref _owners);
			if (owners == 0)
			{
				_parent.Dispose();
				_parent = null;
				Heap.Return(this);
			}
			else if (owners < 0)
			{
				throw new InvalidOperationException("Disposing already disposed MemoryOwner");
			}

			return this;
		}
	}
}