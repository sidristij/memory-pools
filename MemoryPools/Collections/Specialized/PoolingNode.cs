using System.Buffers;

namespace MemoryPools.Collections.Specialized
{
	internal abstract class PoolingNode<T> : IPoolingNode<T>
	{
		protected IMemoryOwner<T> _buf;

		public int Length => _buf.Memory.Length;

		public virtual T this[int index]
		{
			get => _buf.Memory.Span[index];
			set => _buf.Memory.Span[index] = value;
		}

		public virtual void Dispose()
		{
			_buf.Dispose();
			_buf = null;
			Next = null;
		}

		public IPoolingNode<T> Next { get; set; }

		public abstract IPoolingNode<T> Init(int capacity);

		public void Clear()
		{
			_buf.Memory.Span.Clear();
		}
	}
}