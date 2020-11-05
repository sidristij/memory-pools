namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static IPoolingEnumerable<T> Union<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second)
        {
            return Pool.Get<UnionExprEnumerable<T>>().Init(source, second);
        }
    }
}
