using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	internal sealed class PoolingNodeCanon<T> : PoolingNodeBase<object>, IPoolingNode<T> where T : class
	{
		IPoolingNode<T> IPoolingNode<T>.Next
		{
			get => (IPoolingNode<T>) Next;
			set => Next = (IPoolingNode<object>) value;
		}

		T IPoolingNode<T>.this[int index]
		{
			get => (T)_buf.Memory.Span[index];
			set => _buf.Memory.Span[index] = value;
		}

		IPoolingNode<T> IPoolingNode<T>.Init(int capacity)
		{
			this.Init(capacity);
			return this;
		}

		public override void Dispose()
		{
			base.Dispose();
			ObjectsPool<PoolingNodeCanon<T>>.Return(this);
		}

		public override IPoolingNode<object> Init(int capacity)
		{
			Next = null;
			_buf = Pool.GetBuffer<object>(capacity);
			return this;
		}
	}
}