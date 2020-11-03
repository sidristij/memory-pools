using System;
using System.Buffers;

namespace MemoryPools.Memory.Pooling
{
	/// <summary>
	///     This pool returns NEW instance if pool is empty and old if non-empty.
	///     When got, user can return instance back to pool. If not returned, GC will collect it
	///     This means, that if you want to detect 'leaks', you should:
	///     1) add [CallerFilePath], [CallerLineNumber] to your Init() method parameters
	///     2) make finalizer in your type and log saved fileName and lineNumber from (1).
	/// </summary>
	/// <code>
	///  MyType Init(int arg0, string arg1 
	/// 		#if DEBUG
	///  	, [CallerFilePath] string fileName = default, [CallerLineNumber] int lineNumber = default
	/// 		#endif
	///   )
	///  {
	/// 		#if DEBUG
	///  	_fileName = fileName;
	///  	_lineNumber = lineNumber;
	/// 		#endif
	///  }
	/// 	#if DEBUG
	///  ~MyType()
	///  {
	///  	Console.WriteLine($" - {_fileName}:{_lineNumber}");
	///  }
	/// 	#endif
	///  </code>
	public class BucketsBasedCrossThreadsMemoryPool<T> : MemoryPool<T>
	{
		private BucketsBasedCrossThreadsArrayPool<T> _pool;

		internal BucketsBasedCrossThreadsArrayPool<T> _arraysPool => _pool ??= new BucketsBasedCrossThreadsArrayPool<T>();

		public override int MaxBufferSize => Utilities.GetMaxSizeForBucket(17);

		protected override void Dispose(bool disposing)
		{
			;
		}

		public override IMemoryOwner<T> Rent(int minBufferSize = -1)
		{
			return Heap.Get<BufferHolder>().Init(_arraysPool.Rent(minBufferSize), this);
		}

		private class BufferHolder : IMemoryOwner<T>
		{
			private T[] _arr;
			private BucketsBasedCrossThreadsMemoryPool<T> _that;

			public void Dispose()
			{
				_that._arraysPool.Return(_arr);
				_that = null;
				_arr = null;
				Heap.Return(this);
			}

			public Memory<T> Memory => _arr.AsMemory();

			public BufferHolder Init(T[] arr, BucketsBasedCrossThreadsMemoryPool<T> that)
			{
				_arr = arr;
				_that = that;
				return this;
			}
		}
	}
}