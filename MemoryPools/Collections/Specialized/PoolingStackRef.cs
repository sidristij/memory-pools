using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// Collection, which is working on shared btw all Pooling* collections buckets
	/// </summary>
	public class PoolingStackRef<T> : PoolingStack<T> where T : class
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return (IPoolingNode<T>) Heap.Get<PoolingNodeRef<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}