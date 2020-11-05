using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static bool Any<T>(this IPoolingEnumerable<T> source)
        {
            throw new NotImplementedException();
        }
        
        public static bool Any<T, TR>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }
		
        public static bool Any<T, TR, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, TR> condition) where TContext : struct
        {
            throw new NotImplementedException();
        }
        
        public static bool All<T, TR>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }
		
        public static bool All<T, TR, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, TR> condition) where TContext : struct
        {
            throw new NotImplementedException();
        }
    }
}