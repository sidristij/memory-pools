using System;
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
            ObjectsPool<DistinctExprEnumerable<T, T>>.Get().Init(source.GetEnumerator(), x => x);

        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values and selector to select key to compare by. Complexity - O(N)  
        /// </summary>
        public static IPoolingEnumerable<T> DistinctBy<T, TItem>(
            this IPoolingEnumerable<T> source,
            Func<T, TItem> selector) =>
            ObjectsPool<DistinctExprEnumerable<T, TItem>>.Get().Init(source.GetEnumerator(), selector);

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values. Complexity - O(N)
        /// </summary>
        public static IPoolingEnumerable<T> Distinct<T>(
            this IPoolingEnumerable<T> source,
            IEqualityComparer<T> comparer) => 
            ObjectsPool<DistinctExprEnumerable<T, T>>.Get().Init(source.GetEnumerator(), x => x,comparer);

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values and selector to select key to compare by. Complexity - O(N)
        /// </summary>
        public static IPoolingEnumerable<T> DistinctBy<T, TItem>(
            this IPoolingEnumerable<T> source,
            Func<T, TItem> selector,
            IEqualityComparer<TItem> comparer) => 
            ObjectsPool<DistinctExprEnumerable<T, TItem>>.Get().Init(source.GetEnumerator(), selector, comparer);
    }
}