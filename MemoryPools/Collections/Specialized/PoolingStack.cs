namespace MemoryPools.Collections.Specialized
{
	public class PoolingStack<T> : PoolingStackBase<T>
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Pool.Get<PoolingNode<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}