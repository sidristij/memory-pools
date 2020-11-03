using System;
using System.Runtime.CompilerServices;

namespace MemoryPools.Collections.Specialized
{
    public abstract class PoolingStack<T> : IDisposable 
    {
        private IPoolingNode<T> _top;
        private int _topIndex;

        protected PoolingStack()
        {
            Count = 0;
            _topIndex = 0;
            _top = null;
        }

        public bool IsEmpty => Count == 0;

        public int Count { get; private set; }

        public void Dispose()
        {
            Clear();
        }

        protected abstract IPoolingNode<T> CreateNodeHolder();
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T obj)
        {
            if (Count == 0 && _top == null)
                _top = CreateNodeHolder();

            _top[_topIndex] = obj;
            _topIndex++;
            Count++;

            if (_topIndex == PoolsDefaults.DefaultPoolBucketSize)
            {
                var top = _top;
                _top = CreateNodeHolder();
                _top.Next = top;
                _topIndex = 0;
            }
        }

        /// <summary>
        ///     Tries to return queue element if any available via `val` parameter.
        /// </summary>
        /// <returns>
        ///     true if element found or false otherwise
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out T val)
        {
            if (IsEmpty)
            {
                val = default;
                return false;
            }

            val = Pop();
            return true;
        }

        /// <summary>
        ///     Returns queue element
        /// </summary>
        /// <returns>
        ///     Returns element or throws IndexOutOfRangeException if no element found
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Pop()
        {
            if (IsEmpty) throw new IndexOutOfRangeException();

            _topIndex--;

            if (_topIndex < 0)
            {
                _topIndex = PoolsDefaults.DefaultPoolBucketSize - 1;
                var oldTop = _top;
                _top = _top.Next;
                oldTop.Dispose();
            }
            
            var obj = _top[_topIndex];
            _top[_topIndex] = default;

            Count--;

            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            while (_top != null)
            {
                var next = _top.Next;
                _top.Dispose();
                _top = next;
            }
        }
    }
}