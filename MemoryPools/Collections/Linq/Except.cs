using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values. Complexity - O(N)  
        /// </summary>
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except)
        {
            var exceptDict = Pool.Get<PoolingDictionary<T, int>>().Init(0);
            foreach (var item in except) exceptDict[item] = 1;
            
            return Pool.Get<ExceptExprEnumerable<T>>().Init(source, exceptDict);
        } 
        
        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values. Complexity - O(N)
        /// </summary>
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except, IEqualityComparer<T> comparer)
        {
            var exceptDict = Pool.Get<PoolingDictionary<T, int>>().Init(0);
            foreach (var item in except) exceptDict[item] = 1;

            return Pool.Get<ExceptExprEnumerable<T>>().Init(source, exceptDict, comparer);
        }
    }
}