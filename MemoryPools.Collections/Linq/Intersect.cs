using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Intersect<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> intersectWith)
        {
            var second = Pool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in intersectWith) second[item] = 1;
            
            return Pool<IntersectExprEnumerable<T>>.Get().Init(source, second);
        } 
        
        public static IPoolingEnumerable<T> Intersect<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> intersectWith, IEqualityComparer<T> comparer)
        {
            var second = Pool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in intersectWith) second[item] = 1;

            return Pool<IntersectExprEnumerable<T>>.Get().Init(source, second, comparer);
        }
    }
}