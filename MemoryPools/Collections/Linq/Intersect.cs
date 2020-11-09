using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Intersect<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> intersectWith)
        {
            var second = Pool.Get<PoolingDictionary<T, int>>().Init(0);
            foreach (var item in intersectWith) second[item] = 1;
            
            return Pool.Get<IntersectExprEnumerable<T>>().Init(source, second);
        } 
        
        public static IPoolingEnumerable<T> Intersect<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> intersectWith, IEqualityComparer<T> comparer)
        {
            var second = Pool.Get<PoolingDictionary<T, int>>().Init(0);
            foreach (var item in intersectWith) second[item] = 1;

            return Pool.Get<IntersectExprEnumerable<T>>().Init(source, second, comparer);
        }
    }
}