namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// 	List of elements (should be disposed to avoid memory traffic). Max size = 128*128 = 16,384 elements.
	/// 	The best for scenarios, where you need to collect list of elements, use them and forget (w/out removal or inserts).
	/// 	Add: O(1), Insert, Removal: O(N)
	/// </summary>
	public sealed class PoolingListCanon<T> : PoolingListBase<T> where T : class
	{
		public PoolingListCanon() => Init();

		public PoolingListCanon<T> Init()
		{
			_root = Pool.GetBuffer<IPoolingNode<T>>(PoolsDefaults.DefaultPoolBucketSize);
			_ver = 0;
			return this;
		}
		
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return (IPoolingNode<T>) Pool<PoolingNodeCanon<T>>.Get().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}