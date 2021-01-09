namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Returns all elements from <paramref name="source"/> and all -- from <paramref name="second"/>. Complexity = O(N+M)
        /// </summary>
        public static IPoolingEnumerable<T> Concat<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second) => 
            Pool<ConcatExprEnumerable<T>>.Get().Init(source, second);
    }
}
