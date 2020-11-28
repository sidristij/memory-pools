using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values. Complexity - O(N)  
        /// </summary>
        public static IPoolingEnumerable<T> Distinct<T>(this IPoolingEnumerable<T> source) => 
            ObjectsPool<DistinctExprEnumerable<T>>.Get().Init(source.GetEnumerator());

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values. Complexity - O(N)
        /// </summary>
        public static IPoolingEnumerable<T> Distinct<T>(this IPoolingEnumerable<T> source, IEqualityComparer<T> comparer) => 
            ObjectsPool<DistinctExprEnumerable<T>>.Get().Init(source.GetEnumerator(), comparer);
    }
}