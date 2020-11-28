using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace InternalPoolPrefTest
{
	internal sealed class InternalObjectsPoolThreadAwareStack<T> where T : class, new()
	{
		private readonly Stack<T> _freeObjectsQueue = new Stack<T>(128);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get()
		{
			if (_freeObjectsQueue.Count > 0)
				return _freeObjectsQueue.Pop();
			else
				return new T();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Return(T instance) => _freeObjectsQueue.Push(instance);
	}
}