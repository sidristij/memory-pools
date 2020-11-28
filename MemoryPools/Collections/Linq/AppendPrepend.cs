using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Prepend<T>(this IPoolingEnumerable<T> source, T element) => 
            ObjectsPool<PrependExprEnumerable<T>>.Get().Init(source, element);

        public static IPoolingEnumerable<T> Append<T>(this IPoolingEnumerable<T> source, T element) => 
            ObjectsPool<AppendExprEnumerable<T>>.Get().Init(source, element);
    }
}