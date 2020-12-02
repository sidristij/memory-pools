using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.ObjectPool;

namespace MemoryPools.Memory
{
	public sealed class ObjectsPool<T> where T : class, new()
	{
		private static readonly DefaultObjectPool<T> _freeObjectsQueue = new DefaultObjectPool<T>(new DefaultPooledObjectPolicy<T>());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get() => _freeObjectsQueue.Get();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T instance) => _freeObjectsQueue.Return(instance);
	}
}