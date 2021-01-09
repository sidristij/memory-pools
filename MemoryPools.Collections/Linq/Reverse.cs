using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Returns sequence with backward direction. Complexity = 2 * O(N) (collect + return)
        /// </summary>
        public static IPoolingEnumerable<T> Reverse<T>(this IPoolingEnumerable<T> source)
        {
            var list = Pool<PoolingList<T>>.Get().Init();
            foreach (var item in source)
            {
                list.Add(item);
            }
            return Pool<ReverseExprEnumerable<T>>.Get().Init(list);
        }
    }
}