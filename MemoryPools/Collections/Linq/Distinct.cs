using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values. Complexity - O(N)  
        /// </summary>
        public static IPoolingEnumerable<T> Distinct<T>(this IPoolingEnumerable<T> source)
        {
            return Pool.Get<DistinctExprEnumerable<T>>().Init(source.GetEnumerator());
        } 
        
        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values. Complexity - O(N)
        /// </summary>
        public static IPoolingEnumerable<T> Distinct<T>(this IPoolingEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            return Pool.Get<DistinctExprEnumerable<T>>().Init(source.GetEnumerator(), comparer);
        }
    }
}