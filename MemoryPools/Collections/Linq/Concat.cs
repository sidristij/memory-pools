namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Concat<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second)
        {
            return Pool.Get<ConcatExprEnumerable<T>>().Init(source, second);
        }
    }
}
