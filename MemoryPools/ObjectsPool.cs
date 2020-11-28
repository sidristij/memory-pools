using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MemoryPools.Memory
{
	public sealed class ObjectsPool<T> where T : class, new()
	{
		[ThreadStatic]
		private static Stack<T> _freeObjectsQueue;
		
		private static Stack<T> FreeObjectsQueue => _freeObjectsQueue ??= new Stack<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get() => FreeObjectsQueue.Count > 0 ? FreeObjectsQueue.Pop() : new T();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T instance) => FreeObjectsQueue.Push(instance);
	}
}