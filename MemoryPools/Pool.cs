using System;
using System.Runtime.CompilerServices;
using MemoryPools.Memory;
using Microsoft.Extensions.ObjectPool;

namespace MemoryPools
{
	public static class Pool<T> where T : class, new()
	{
		private static readonly DefaultObjectPool<T> _freeObjectsQueue = new DefaultObjectPool<T>(new DefaultPooledObjectPolicy<T>());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get()
		{
			return _freeObjectsQueue.Get();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return<T1>(T1 instance) where T1 : T
		{
			_freeObjectsQueue.Return(instance);
		}
	}

	public static class Pool
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get<T>() where T : class, new()
		{
			return Pool<T>.Get();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return<T>(T instance) where T : class, new()
		{
			Pool<T>.Return(instance);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBuffer<T>(int size)
		{
			return InternalArraysPool.Rent<T>(size, false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CountdownMemoryOwner<T> GetBufferFrom<T>(ReadOnlySpan<T> source)
		{
			return InternalArraysPool.RentFrom(source, false);
		}
	}
}