namespace MemoryPools.Collections.Specialized
{
	public sealed class PoolingListVal<T> : PoolingListBase<PoolingListVal<T>, T> where T : struct
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Pool.Get<PoolingNodeVal<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}