using System.Collections.Generic;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Union<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second)
        {
            var set = ObjectsPool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in source) set[item] = 1;
            foreach (var item in second) set[item] = 1;
            
            return ObjectsPool<UnionExprEnumerable<T>>.Get().Init(set);
        } 
        
        public static IPoolingEnumerable<T> Union<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            var set = ObjectsPool<PoolingDictionary<T, int>>.Get().Init(0, comparer);
            foreach (var item in source) set[item] = 1;
            foreach (var item in second) set[item] = 1;
            
            return ObjectsPool<UnionExprEnumerable<T>>.Get().Init(set);
        }
    }
}