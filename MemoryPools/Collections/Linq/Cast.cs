namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Casts all elements to the given type. Complexity = O(N)
        /// </summary>
        public static IPoolingEnumerable<TR> Cast<TR>(this IPoolingEnumerable source)
        {
	        if (source is IPoolingEnumerable<TR> res) return res;
            return Pool.Get<CastExprEnumerable<TR>>().Init(source);
        }
    }
}