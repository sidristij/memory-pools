namespace MemoryPools.Collections.Specialized
{
	internal sealed class PoolingNodeVal<T> : PoolingNode<T> where T : struct
	{
		public override void Dispose()
		{
			base.Dispose();
			Heap.Return(this);
		}

		public override IPoolingNode<T> Init(int capacity)
		{
			Next = null;
			_buf = Heap.GetBuffer<T>(capacity);
			return this;
		}
	}
}