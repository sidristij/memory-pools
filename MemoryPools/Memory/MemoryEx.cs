using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace MemoryPools.Memory
{
	public static class MemoryEx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Length<T>(this IMemoryOwner<T> that) => 
			that.Memory.Length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> AsCountdown<T>(this CountdownMemoryOwner<T> that, bool noDefaultOwner = false) => 
			ObjectsPool<CountdownMemoryOwner<T>>.Get().Init(that, 0, that.Memory.Length, noDefaultOwner);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> AsCountdown<T>(this CountdownMemoryOwner<T> that, int offset,
			bool noDefaultOwner = false) =>
			ObjectsPool<CountdownMemoryOwner<T>>.Get().Init(that, offset, that.Memory.Length - offset, noDefaultOwner);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> AsCountdown<T>(this CountdownMemoryOwner<T> that, int offset, int length, bool noDefaultOwner = false) => 
			ObjectsPool<CountdownMemoryOwner<T>>.Get().Init(that, offset, length, noDefaultOwner);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMemoryOwner<T> Slice<T>(this CountdownMemoryOwner<T> that, int offset) => 
			Slice(that, offset, that.Memory.Length - offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMemoryOwner<T> Slice<T>(this CountdownMemoryOwner<T> that, int offset, int length) => 
			that.AsCountdown(offset, length);
	}
}