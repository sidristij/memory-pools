using System;
using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<IPoolingGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IPoolingEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            ObjectsPool<GroupedEnumerable<TSource, TKey, TSource>>.Get().Init(source, keySelector, x => x, null);
        
        public static IPoolingEnumerable<IPoolingGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IPoolingEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            ObjectsPool<GroupedEnumerable<TSource, TKey, TSource>>.Get().Init(source, keySelector, x => x, comparer);

        public static IPoolingEnumerable<IPoolingGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IPoolingEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
            ObjectsPool<GroupedEnumerable<TSource, TKey, TElement>>.Get().Init(source, keySelector, elementSelector, null);

        public static IPoolingEnumerable<IPoolingGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IPoolingEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) =>
            ObjectsPool<GroupedEnumerable<TSource, TKey, TElement>>.Get().Init(source, keySelector, elementSelector, comparer);

        public static IPoolingEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IPoolingEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IPoolingEnumerable<TSource>, TResult> resultSelector) =>
            ObjectsPool<GroupedResultEnumerable<TSource, TKey, TResult>>.Get().Init(source, keySelector, resultSelector, null);
        
        // public static IPoolingEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector) =>
        //     new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, null);
        
        public static IPoolingEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IPoolingEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IPoolingEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            ObjectsPool<GroupedResultEnumerable<TSource, TKey, TResult>>.Get().Init(source, keySelector, resultSelector, comparer);
        
        // public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
        //     new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
    }
}