using System.Buffers;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Memory.Pooling
{
	public class BucketsBasedCrossThreadsArrayPool<T> : ArrayPool<T>
	{
		private static MemoryPool<T> _shared;
		private static readonly PoolingQueueRef<T[]>[] _pool = new PoolingQueueRef<T[]>[24];

		static BucketsBasedCrossThreadsArrayPool()
		{
			for (var i = 0; i < _pool.Length; i++) _pool[i] = new PoolingQueueRef<T[]>();
		}

		public new static MemoryPool<T> Shared => _shared ??= new BucketsBasedCrossThreadsMemoryPool<T>();

		public override T[] Rent(int minimumLength)
		{
			var queueIndex = Utilities.GetBucket(minimumLength);
			lock (_pool)
			{
				var queue = _pool[queueIndex];
				T[] arr;

				if (queue.Count == 0)
				{
					var length = Utilities.GetMaxSizeForBucket(Utilities.GetBucket(minimumLength));
					arr = new T[length];
					return arr;
				}

				arr = queue.Dequeue();
				return arr;
			}
		}

		public override void Return(T[] array, bool clearArray = false)
		{
			lock (_pool)
			{
				var queueIndex = Utilities.GetBucket(array.Length);
				_pool[queueIndex].Enqueue(array);
			}
		}
	}
}