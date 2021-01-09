namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// Collection, which is working on shared btw all Pooling* collections buckets
	/// </summary>
	public class PoolingStackCanon<T> : PoolingStackBase<T> where T : class
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return (IPoolingNode<T>) Pool<PoolingNodeCanon<T>>.Get().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}