namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static IPoolingEnumerable<(T, T)> Zip<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second)
        {
            return Pool.Get<ZipExprEnumerable<T>>().Init(source, second);
        }
    }
}