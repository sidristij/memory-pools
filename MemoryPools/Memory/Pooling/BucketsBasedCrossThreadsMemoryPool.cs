using System;
using System.Runtime.CompilerServices;

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
	public sealed class BucketsBasedCrossThreadsMemoryPool<T>
	{
		private BucketsBasedCrossThreadsArrayPool<T> _pool;

		[ThreadStatic]
		private static BucketsBasedCrossThreadsMemoryPool<T> _mempool;

		internal BucketsBasedCrossThreadsArrayPool<T> _arraysPool => _pool ??= new BucketsBasedCrossThreadsArrayPool<T>();

		public static BucketsBasedCrossThreadsMemoryPool<T> Shared => _mempool ??= new BucketsBasedCrossThreadsMemoryPool<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CountdownMemoryOwner<T> Rent(int minBufferSize = -1)
		{
			return Pool<CountdownMemoryOwner<T>>.Get().Init(_arraysPool.Rent(minBufferSize), minBufferSize);
		}
	}
}