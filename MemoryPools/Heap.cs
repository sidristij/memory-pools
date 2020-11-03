using System;
using System.Runtime.CompilerServices;
using MemoryPools.Memory;

namespace MemoryPools
{
	public static class Heap
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get<T>() where T : class, new() =>
			InternalObjectsPool<T>.Get();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return<T>(T instance) where T : class, new() =>
			InternalObjectsPool<T>.Return(instance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBuffer<T>(int size) =>
			Arrays.Rent<T>(size, false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBuffer<T>(int size, bool noDefaultOwner) =>
			Arrays.Rent<T>(size, noDefaultOwner);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBufferFrom<T>(ReadOnlySpan<T> source) where T : struct =>
			Arrays.RentFrom(source, false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBufferFrom<T>(ReadOnlySpan<T> source, bool noDefaultOwner) where T : struct =>
			Arrays.RentFrom(source, noDefaultOwner);
	}
}