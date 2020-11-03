using System;
using System.Buffers;

namespace MemoryPools.Memory
{
	public class MemoryOwner<T> : IMemoryOwner<T>
	{
		public static MemoryOwner<T> Empty = new MemoryOwner<T>(Memory<T>.Empty);

		protected MemoryOwner(Memory<T> memory)
		{
			Memory = memory;
		}

		public void Dispose()
		{
			Memory = Memory<T>.Empty;
		}

		public Memory<T> Memory { get; set; }
	}
}