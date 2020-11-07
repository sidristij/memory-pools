namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// Collection, which is working on shared btw all Pooling* collections buckets
	/// </summary>
	public class PoolingStackCanon<T> : PoolingStackBase<T> where T : class
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return (IPoolingNode<T>) Pool.Get<PoolingNodeCanon<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}