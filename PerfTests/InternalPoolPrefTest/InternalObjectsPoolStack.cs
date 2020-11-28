using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace InternalPoolPrefTest
{
	internal sealed class InternalObjectsPoolStack<T> where T : class, new()
	{
		private static readonly Stack<T> FreeObjectsQueue = new Stack<T>(128);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get()
		{
			lock (FreeObjectsQueue)
			{
				if (FreeObjectsQueue.Count > 0)
					return FreeObjectsQueue.Pop();
				else
					return new T();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T instance)
		{
			lock (FreeObjectsQueue)
			{
				FreeObjectsQueue.Push(instance);
			}
		}
	}
}