using System;

namespace MemoryPools.Collections.Specialized
{
	public interface IPoolingNode<T> : IDisposable
	{
		public IPoolingNode<T> Next { get; set; }

		T this[int index] { get; set; }

		IPoolingNode<T> Init(int capacity);
		
		void Clear();
	}
}