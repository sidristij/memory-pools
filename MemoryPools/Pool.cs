using System;
using System.Runtime.CompilerServices;
using MemoryPools.Memory;

namespace MemoryPools
{
	public static class Pool
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get<T>() where T : class, new() =>
			InternalObjectsPool<T>.Get();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return<T>(T instance) where T : class, new() =>
			InternalObjectsPool<T>.Return(instance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBuffer<T>(int size) =>
			InternalArraysPool.Rent<T>(size, false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBuffer<T>(int size, bool noDefaultOwner) =>
			InternalArraysPool.Rent<T>(size, noDefaultOwner);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBufferFrom<T>(ReadOnlySpan<T> source) where T : struct =>
			InternalArraysPool.RentFrom(source, false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBufferFrom<T>(ReadOnlySpan<T> source, bool noDefaultOwner) where T : struct =>
			InternalArraysPool.RentFrom(source, noDefaultOwner);
	}
}