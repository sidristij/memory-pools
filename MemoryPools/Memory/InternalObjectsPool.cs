using System.Runtime.CompilerServices;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Memory
{
	internal sealed class InternalObjectsPool<T> where T : class, new()
	{
		private static readonly PoolingQueueRef<T> _freeObjectsQueue = new PoolingQueueRef<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get()
		{
			lock (_freeObjectsQueue)
			{
				return _freeObjectsQueue.Count > 0 ? _freeObjectsQueue.Dequeue() : new T();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T instance)
		{
			lock (_freeObjectsQueue)
			{
				_freeObjectsQueue.Enqueue(instance);
			}
		}
	}
}