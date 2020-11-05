namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static IPoolingEnumerable<TR> Cast<TR>(this IPoolingEnumerable source)
        {
	        if (source is IPoolingEnumerable<TR> res) return res;
            return Pool.Get<CastClauseEnumerable<TR>>().Init(source);
        }
    }
}