using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Skip<T>(this IPoolingEnumerable<T> source, int count)
        {
            return ObjectsPool<SkipTakeExprPoolingEnumerable<T>>.Get().Init(source, false, count);
        }

        public static IPoolingEnumerable<T> Take<T>(this IPoolingEnumerable<T> source, int count)
        {
            return ObjectsPool<SkipTakeExprPoolingEnumerable<T>>.Get().Init(source, true, count);
        }
    }
}