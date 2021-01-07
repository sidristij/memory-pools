using System.Collections.Generic;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except)
        {
            var exceptDict = ObjectsPool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in except) exceptDict[item] = 1;
            
            return ObjectsPool<ExceptExprEnumerable<T>>.Get().Init(source, exceptDict);
        } 
        
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except, IEqualityComparer<T> comparer)
        {
            var exceptDict = ObjectsPool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in except) exceptDict[item] = 1;

            return ObjectsPool<ExceptExprEnumerable<T>>.Get().Init(source, exceptDict, comparer);
        }
    }
}