namespace MemoryPools.Collections.Specialized
{
	public sealed class PoolingList<T> : PoolingListBase<T>
	{
		public PoolingList() => Init();

		public PoolingList<T> Init()
		{
			_root = Pool.GetBuffer<IPoolingNode<T>>(PoolsDefaults.DefaultPoolBucketSize);
			_ver = 0;
			return this;
		}
		
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Pool.Get<PoolingNode<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}