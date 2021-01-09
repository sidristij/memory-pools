namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        // public static IPoolingEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
        //     this IPoolingEnumerable<TOuter> outer, 
        //     IPoolingEnumerable<TInner> inner, 
        //     Func<TOuter, TKey> outerKeySelector, 
        //     Func<TInner, TKey> innerKeySelector, 
        //     Func<TOuter, TInner, TResult> resultSelector)
        // {
        //     if (outer == null)
        //     {
        //         throw new ArgumentNullException(nameof(outer));
        //     }
        //
        //     if (inner == null)
        //     {
        //         throw new ArgumentNullException(nameof(inner));
        //     }
        //
        //     if (outerKeySelector == null)
        //     {
        //         throw new ArgumentNullException(nameof(outerKeySelector));
        //     }
        //
        //     if (innerKeySelector == null)
        //     {
        //         throw new ArgumentNullException(nameof(innerKeySelector));
        //     }
        //
        //     if (resultSelector == null)
        //     {
        //         throw new ArgumentNullException(nameof(resultSelector));
        //     }
        //
        //     return JoinIterator(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
        // }
        //
        // public static IPoolingEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
        //     this IEnumerable<TOuter> outer,
        //     IEnumerable<TInner> inner,
        //     Func<TOuter, TKey> outerKeySelector,
        //     Func<TInner, TKey> innerKeySelector,
        //     Func<TOuter, TInner, TResult> resultSelector,
        //     IEqualityComparer<TKey> comparer)
        // {
        //     if (outer == null)
        //     {
        //         throw new ArgumentNullException(nameof(outer));
        //     }
        //
        //     if (inner == null)
        //     {
        //         throw new ArgumentNullException(nameof(inner));
        //     }
        //
        //     if (outerKeySelector == null)
        //     {
        //         throw new ArgumentNullException(nameof(outerKeySelector));
        //     }
        //
        //     if (innerKeySelector == null)
        //     {
        //         throw new ArgumentNullException(nameof(innerKeySelector));
        //     }
        //
        //     if (resultSelector == null)
        //     {
        //         throw new ArgumentNullException(nameof(resultSelector));
        //     }
        //
        //     return JoinIterator(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        // }

        // private static IPoolingEnumerable<TResult> JoinIterator<TOuter, TInner, TKey, TResult>(
        //     IPoolingEnumerable<TOuter> outer, 
        //     IEnumerable<TInner> inner,
        //     Func<TOuter, TKey> outerKeySelector,
        //     Func<TInner, TKey> innerKeySelector,
        //     Func<TOuter, TInner, TResult> resultSelector,
        //     IEqualityComparer<TKey> comparer)
        // {
        //     using (var e = outer.GetEnumerator())
        //     {
        //         var dict = InternalPool<PoolingDictionary<TOuter, TKey>>.Get();
        //         if (e.MoveNext())
        //         {
        //             Lookup<TKey, TInner> lookup = Lookup<TKey, TInner>.CreateForJoin(inner, innerKeySelector, comparer);
        //             if (lookup.Count != 0)
        //             {
        //                 do
        //                 {
        //                     TOuter item = e.Current;
        //                     Grouping<TKey, TInner> g = lookup.GetGrouping(outerKeySelector(item), create: false);
        //                     if (g != null)
        //                     {
        //                         int count = g._count;
        //                         TInner[] elements = g._elements;
        //                         for (int i = 0; i != count; ++i)
        //                         {
        //                             yield return resultSelector(item, elements[i]);
        //                         }
        //                     }
        //                 }
        //                 while (e.MoveNext());
        //             }
        //         }
        //     }
        // }
    }
}