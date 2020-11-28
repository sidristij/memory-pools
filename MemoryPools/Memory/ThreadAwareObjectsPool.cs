using System.Runtime.CompilerServices;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Memory
{
	public sealed class ThreadAwareObjectsPool<T> where T : class, new()
	{
		private readonly PoolingQueueRef<T> _freeObjectsQueue = new PoolingQueueRef<T>();

		/// <summary>
		/// Internal ctor because we can do some args in future with backward compatibility
		/// </summary>
		internal ThreadAwareObjectsPool()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get()
		{
			if (_freeObjectsQueue.Count > 0)
				return _freeObjectsQueue.Dequeue();
			else
				return new T();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Return(T instance)
		{
			_freeObjectsQueue.Enqueue(instance);
		}
	}
}