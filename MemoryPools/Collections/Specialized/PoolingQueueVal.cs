using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	///     Poolinq queue stores items in buckets of 256 size, who linked with linked list.
	///     Nodes of this list and storage (array[256])
	///     ** NOT THREAD SAFE **
	/// 	Enqueue, dequeue: O(1). 
	/// </summary>
	/// <typeparam name="T">Items should be classes because underlying collection stores object type</typeparam>
	public sealed class PoolingQueueVal<T> : PoolingQueue<T> where T : struct
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Heap.Get<PoolingNodeVal<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}