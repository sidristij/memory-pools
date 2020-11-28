using System;
using System.Collections.Generic;

namespace MemoryPools.Memory.Pooling
{
	public sealed class BucketsBasedCrossThreadsArrayPool<T>
	{
		[ThreadStatic]
		private static BucketsBasedCrossThreadsArrayPool<T> _shared;
		private static readonly Queue<T[]>[] _pool = new Queue<T[]>[24];

		static BucketsBasedCrossThreadsArrayPool()
		{
			for (var i = 0; i < _pool.Length; i++) _pool[i] = new Queue<T[]>();
		}

		public static BucketsBasedCrossThreadsArrayPool<T> Shared => _shared ??= new BucketsBasedCrossThreadsArrayPool<T>();

		public T[] Rent(int minimumLength)
		{
			var queueIndex = Utilities.GetBucket(minimumLength);
			var queue = _pool[queueIndex];
			T[] arr;

			if (queue.Count == 0)
			{
				var length = Utilities.GetMaxSizeForBucket(queueIndex);
				arr = new T[length];
				return arr;
			}

			arr = queue.Dequeue();
			return arr;
		}

		public void Return(T[] array)
		{
			_pool[Utilities.GetBucket(array.Length)].Enqueue(array);
		}
	}
}