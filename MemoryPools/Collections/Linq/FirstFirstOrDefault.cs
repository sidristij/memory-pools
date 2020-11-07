using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static T First<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            var hasItems = enumerator.MoveNext();
            if (!hasItems)
            {
                throw new InvalidOperationException("Sequence is empty");
            }
            var element = enumerator.Current;
            enumerator.Dispose();
            return element;
        }
        
        public static T First<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                {
                    var item = enumerator.Current;
                    enumerator.Dispose();
                    return item;
                }
            }
            enumerator.Dispose();
            throw new InvalidOperationException("Sequence is empty");
        }
		
        public static T First<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(context, enumerator.Current))
                {
                    var item = enumerator.Current;
                    enumerator.Dispose();
                    return item;
                }
            }
            enumerator.Dispose();
            throw new InvalidOperationException("Sequence is empty");
        }
        
        public static T FirstOrDefault<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            var hasItem = enumerator.MoveNext();

            var item= hasItem ? enumerator.Current : default;
            enumerator.Dispose();

            return item;
        }
        
        public static T FirstOrDefault<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                {
                    var elem = enumerator.Current;
                    enumerator.Dispose();
                    return elem;
                }
            }
            enumerator.Dispose();
            return default;

        }
		
        public static T FirstOrDefault<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(context, enumerator.Current))
                {
                    var elem = enumerator.Current;
                    enumerator.Dispose();
                    return elem;
                }
            }
            enumerator.Dispose();
            return default;
        }
    }
}