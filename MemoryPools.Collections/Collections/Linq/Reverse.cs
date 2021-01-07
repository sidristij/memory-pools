using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Returns sequence with backward direction. Complexity = 2 * O(N) (collect + return)
        /// </summary>
        public static IPoolingEnumerable<T> Reverse<T>(this IPoolingEnumerable<T> source)
        {
            var list = ObjectsPool<PoolingList<T>>.Get().Init();
            foreach (var item in source)
            {
                list.Add(item);
            }
            return ObjectsPool<ReverseExprEnumerable<T>>.Get().Init(list);
        }
    }
}