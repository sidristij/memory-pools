using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static bool Any<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            var hasItems = enumerator.MoveNext();
            enumerator.Dispose();
            return hasItems;
        }
        
        public static bool Any<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                {
                    enumerator.Dispose();
                    return true;
                }
            }
            enumerator.Dispose();
            return false;
        }
		
        public static bool Any<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) where TContext : struct
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(context, enumerator.Current))
                {
                    enumerator.Dispose();
                    return true;
                }
            }
            enumerator.Dispose();
            return false;
        }
        
        public static bool All<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!condition(enumerator.Current))
                {
                    enumerator.Dispose();
                    return false;
                }
            }
            enumerator.Dispose();
            return true;

        }
		
        public static bool All<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) where TContext : struct
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!condition(context, enumerator.Current))
                {
                    enumerator.Dispose();
                    return false;
                }
            }
            enumerator.Dispose();
            return true;
        }
    }
}