using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// 	List of elements (should be disposed to avoid memory traffic). Max size = 128*128 = 16,384 elements.
	/// 	The best for scenarios, where you need to collect list of elements, use them and forget (w/out removal or inserts).
	/// 	Add: O(1), Insert, Removal: O(N)
	/// </summary>
	public sealed class PoolingListRef<T> : PoolingListBase<PoolingListRef<T>, T> where T : class
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return (IPoolingNode<T>) Heap.Get<PoolingNodeRef<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}