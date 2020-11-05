namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static IPoolingEnumerable<TR> OfType<TR>(this IPoolingEnumerable source)
        {
            if (source is IPoolingEnumerable<TR> res) return res;
            return Pool.Get<OfTypeClauseEnumerable<TR>>().Init(source);
        }
    }
}