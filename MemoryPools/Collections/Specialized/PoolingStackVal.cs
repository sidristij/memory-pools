﻿namespace MemoryPools.Collections.Specialized
{
	public class PoolingStackVal<T> : PoolingStack<T> where T : struct
	{
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Pool.Get<PoolingNodeVal<T>>().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}