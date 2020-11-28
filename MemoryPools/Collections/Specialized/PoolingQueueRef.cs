using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public sealed class PoolingQueueRef<T> : PoolingQueue<T> where T : class
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return (IPoolingNode<T>) ObjectsPool<PoolingNodeCanon<T>>.Get().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}