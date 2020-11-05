namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static IPoolingEnumerable<T> Prepend<T>(this IPoolingEnumerable<T> source, T element)
        {
            return Pool.Get<PrependExprEnumerable<T>>().Init(source, element);
        }

        public static IPoolingEnumerable<T> Append<T>(this IPoolingEnumerable<T> source, T element)
        {
            return Pool.Get<AppendExprEnumerable<T>>().Init(source, element);
        }
    }
}