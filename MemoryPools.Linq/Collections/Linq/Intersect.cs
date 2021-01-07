using System.Collections.Generic;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Intersect<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> intersectWith)
        {
            var second = ObjectsPool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in intersectWith) second[item] = 1;
            
            return ObjectsPool<IntersectExprEnumerable<T>>.Get().Init(source, second);
        } 
        
        public static IPoolingEnumerable<T> Intersect<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> intersectWith, IEqualityComparer<T> comparer)
        {
            var second = ObjectsPool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in intersectWith) second[item] = 1;

            return ObjectsPool<IntersectExprEnumerable<T>>.Get().Init(source, second, comparer);
        }
    }
}