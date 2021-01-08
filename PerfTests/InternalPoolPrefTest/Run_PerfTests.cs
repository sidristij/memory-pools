using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using MemoryPools;
using Microsoft.Extensions.ObjectPool;

namespace InternalPoolPrefTest
{
	class Program
	{
		public static void Main(string[] args) => BenchmarkRunner.Run<Run_PerfTests>();
	}

	[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
	public class Run_PerfTests
	{
		readonly DefaultObjectPool<PoolItem> _mspool = new DefaultObjectPool<PoolItem>(new DefaultPooledObjectPolicy<PoolItem>());
		readonly JetPool<PoolItem> _jmypool = new JetPool<PoolItem>();
		readonly SuperJetPool<PoolItem> _sjmypool = new SuperJetPool<PoolItem>();

		[Benchmark(Description = "Microsoft.Extensions.ObjectsPool")]
		public void TestInternalArrayPool_MSObjectsPool()
		{
			_mspool.Return(_mspool.Get());
		}
		
		[Benchmark(Description = "MemoryPooling.ObjectsPool")]
		public void TestInternalArrayPool_ThreadAwareQueue()
		{
			Pool<PoolItem>.Return(Pool<PoolItem>.Get());
		}
		
		[Benchmark(Description = "Own, thread-aware")]
		public void TestInternalArrayPool_JetStack()
		{
			_jmypool.Return(_jmypool.Get());
		}
		
		[Benchmark(Description = "Own, thread-aware+hack")]
		public void TestInternalArrayPool_SuperJetStack()
		{
			_sjmypool.Return(_sjmypool.Get());
		}
		
		[Benchmark(Description = "Regular new()")]
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public void TestInternalArrayPool_New()
		{
			var x = new PoolItem();
		}
	}
	class PoolItem
	{
		public int x;
	}
	
	public sealed class JetPool<T> where T : class, new()
	{
		private readonly JetStack<T> _freeObjectsQueue = new JetStack<T>();

		internal JetPool()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get() => this._freeObjectsQueue.Count > 0 ? this._freeObjectsQueue.Pop() : new T();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Return(T instance) => this._freeObjectsQueue.Push(instance);
	}	
	
	public sealed class SuperJetPool<T> where T : class, new()
	{
		private readonly SuperJetStack<T> _freeObjectsQueue = new SuperJetStack<T>();

		internal SuperJetPool()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get() => this._freeObjectsQueue.Count > 0 ? this._freeObjectsQueue.Pop() : new T();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Return(T instance) => this._freeObjectsQueue.Push(instance);
	}
	
	public class JetStack<T>
	{
		private T[] _array;
		private int _size;
    
		private const int DefaultCapacity = 4;

		public JetStack()
		{
			_array = new T[DefaultCapacity];
		}

		// Create a stack with a specific initial capacity.  The initial capacity
		// must be a non-negative number.
		public JetStack(int capacity)
		{
			_array = new T[capacity];
		}

		public int Count => _size;
        
		// Removes all Objects from the Stack.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			Array.Clear(_array, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
			_size = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Peek() => _array[_size - 1];

		// Pops an item from the top of the stack.  If the stack is empty, Pop
		// throws an InvalidOperationException.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Pop()
		{
			return _array[--_size];
		}

		// Pushes an item to the top of the stack.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Push(T item)
		{
			if (_size >= _array.Length)
			{
				PushWithResize(item);
			}
			else
			{
				_array[_size++] = item;
			}
		}

		// Non-inline from Stack.Push to improve its code quality as uncommon path
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PushWithResize(T item)
		{
			Array.Resize(ref _array, _array.Length<<1);
			_array[_size] = item;
			_size++;
		}
	}
	public class SuperJetStack<T>
	{
		private ObjectWrapper[] _array;
		
		private int _size;
		private T _firstItem;
    
		private const int DefaultCapacity = 4;

		public SuperJetStack()
		{
			_array = new ObjectWrapper[DefaultCapacity];
		}

		// Create a stack with a specific initial capacity.  The initial capacity
		// must be a non-negative number.
		public SuperJetStack(int capacity)
		{
			_array = new ObjectWrapper[capacity];
		}

		public int Count => _size;
        
		// Removes all Objects from the Stack.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			Array.Clear(_array, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
			_size = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Peek() => _array[_size - 1].Element;

		// Pops an item from the top of the stack.  If the stack is empty, Pop
		// throws an InvalidOperationException.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Pop()
		{
			var item = _firstItem;
			if (_firstItem != null)
			{
				_firstItem = default(T);
				return item;
			}
			return _array[--_size].Element;
		}

		// Pushes an item to the top of the stack.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Push(T item)
		{
			if (_firstItem == null)
			{
				_firstItem = item;
				return;
			}
			if (_size >= _array.Length)
			{
				PushWithResize(item);
			}
			else
			{
				_array[_size++].Element = item;
			}
		}

		// Non-inline from Stack.Push to improve its code quality as uncommon path
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PushWithResize(T item)
		{
			Array.Resize(ref _array, _array.Length<<1);
			_array[_size].Element = item;
			_size++;
		}
		
		// PERF: the struct wrapper avoids array-covariance-checks from the runtime when assigning to elements of the array.
		private struct ObjectWrapper
		{
			public T Element;
		}
	}
}