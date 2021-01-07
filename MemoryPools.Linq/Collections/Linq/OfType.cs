using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<TR> OfType<TR>(this IPoolingEnumerable source)
        {
            if (source is IPoolingEnumerable<TR> res) return res;
            return ObjectsPool<OfTypeExprEnumerable<TR>>.Get().Init(source);
        }
    }
}