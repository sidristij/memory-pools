using System.Runtime.CompilerServices;
using MemoryPools.Memory;

namespace MemoryPools
{
    public class JetPool<T> where T : class, new()
    {
        private readonly JetStack<T> _freeObjectsQueue = new JetStack<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get() => _freeObjectsQueue.Count > 0 ? _freeObjectsQueue.Pop() : new T();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T instance) => _freeObjectsQueue.Push(instance);
    }
}