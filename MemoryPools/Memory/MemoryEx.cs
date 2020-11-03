using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace MemoryPools.Memory
{
	public static class MemoryEx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Length<T>(this IMemoryOwner<T> that)
		{
			return that.Memory.Length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> AsCountdown<T>(this IMemoryOwner<T> that, bool noDefaultOwner = false)
		{
			return Heap.Get<CountdownMemoryOwner<T>>().Init(that, 0, that.Memory.Length, noDefaultOwner);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> AsCountdown<T>(this IMemoryOwner<T> that, int offset,
			bool noDefaultOwner = false)
		{
			return Heap.Get<CountdownMemoryOwner<T>>().Init(that, offset, that.Memory.Length - offset, noDefaultOwner);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> AsCountdown<T>(this IMemoryOwner<T> that, int offset, int length,
			bool noDefaultOwner = false)
		{
			return Heap.Get<CountdownMemoryOwner<T>>().Init(that, offset, length, noDefaultOwner);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMemoryOwner<T> Slice<T>(this IMemoryOwner<T> that, int offset)
		{
			return Slice(that, offset, that.Memory.Length - offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMemoryOwner<T> Slice<T>(this IMemoryOwner<T> that, int offset, int length)
		{
			if (!(that is CountdownMemoryOwner<T>))
				// other types of IMemoryOwner may return Memory instance to pool and give it to other side for use  
				throw new InvalidOperationException(
					$"Passed memory owner type '{that.GetType().FullName}' isn't supported.");
			// wrap it with another countdown to support slicing
			return that.AsCountdown(offset, length);
		}
	}
}