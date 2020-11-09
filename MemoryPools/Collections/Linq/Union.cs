using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Union<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second)
        {
            var set = Pool.Get<PoolingDictionary<T, int>>().Init(0);
            foreach (var item in source) set[item] = 1;
            foreach (var item in second) set[item] = 1;
            
            return Pool.Get<UnionExprEnumerable<T>>().Init(set);
        } 
        
        public static IPoolingEnumerable<T> Union<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            var set = Pool.Get<PoolingDictionary<T, int>>().Init(0, comparer);
            foreach (var item in source) set[item] = 1;
            foreach (var item in second) set[item] = 1;
            
            return Pool.Get<UnionExprEnumerable<T>>().Init(set);
        }
    }
}