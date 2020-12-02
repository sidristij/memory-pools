using System;
using System.Runtime.CompilerServices;
using MemoryPools.Memory;

	namespace MemoryPools
	{
		public static class Pool
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static CountdownMemoryOwner<T> GetBuffer<T>(int size) =>
				InternalArraysPool.Rent<T>(size, false);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static CountdownMemoryOwner<T> GetBufferFrom<T>(ReadOnlySpan<T> source) =>
				InternalArraysPool.RentFrom(source, false);
		}
	}