using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MemoryPools.Memory;
using MemoryPools.Memory.Pooling;

namespace InternalPoolPrefTest
{
	internal sealed class InternalObjectsPoolQueue<T> where T : class, new()
	{
		private static readonly Queue<T> FreeObjectsQueue = new Queue<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get()
		{
			lock (FreeObjectsQueue)
			{
				if (FreeObjectsQueue.Count > 0)
					return FreeObjectsQueue.Dequeue();
				else
					return new T();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T instance)
		{
			lock (FreeObjectsQueue)
			{
				FreeObjectsQueue.Enqueue(instance);
			}
		}
	}
	
	internal sealed class InternalObjectsPoolThreadAware<T> where T : class, new()
	{
		private static readonly Queue<T> FreeObjectsQueue = new Queue<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get()
		{
			if (FreeObjectsQueue.Count > 0)
				return FreeObjectsQueue.Dequeue();
			else
				return new T();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T instance)
		{
			FreeObjectsQueue.Enqueue(instance);
		}
	}
}