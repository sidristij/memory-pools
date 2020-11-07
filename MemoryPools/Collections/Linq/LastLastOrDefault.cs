using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static T Last<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : throw new InvalidOperationException("Sequence is empty");
        }
        
        public static T Last<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : throw new InvalidOperationException("Sequence is empty");
        }
		
        public static T Last<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(context, enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : throw new InvalidOperationException("Sequence is empty");
        }
        
        public static T LastOrDefault<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : default;
        }
        
        public static T LastOrDefault<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : default;
        }
		
        public static T LastOrDefault<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(context, enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : default;
        }
    }
}