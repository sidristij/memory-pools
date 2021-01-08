using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except)
        {
            var exceptDict = Pool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in except) exceptDict[item] = 1;
            
            return Pool<ExceptExprEnumerable<T>>.Get().Init(source, exceptDict);
        } 
        
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except, IEqualityComparer<T> comparer)
        {
            var exceptDict = Pool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in except) exceptDict[item] = 1;

            return Pool<ExceptExprEnumerable<T>>.Get().Init(source, exceptDict, comparer);
        }
    }
}