namespace MemoryPools.Collections.Specialized
{
	internal sealed class PoolingNode<T> : PoolingNodeBase<T>
	{
		public override void Dispose()
		{
			base.Dispose();
			Pool<PoolingNode<T>>.Return(this);
		}

		public override IPoolingNode<T> Init(int capacity)
		{
			Next = null;
			_buf = Pool.GetBuffer<T>(capacity);
			return this;
		}
	}
}