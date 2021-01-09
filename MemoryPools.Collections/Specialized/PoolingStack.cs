namespace MemoryPools.Collections.Specialized
{
	public class PoolingStack<T> : PoolingStackBase<T>
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Pool<PoolingNode<T>>.Get().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}