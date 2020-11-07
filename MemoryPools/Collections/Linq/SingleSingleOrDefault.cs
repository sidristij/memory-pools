using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static T Single<T>(this IPoolingEnumerable<T> source)
        {
            var wasFound = false;
            var element = default(T);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (wasFound)
                {
                    enumerator.Dispose();
                    throw new InvalidOperationException("Sequence should contain only one element");
                }

                wasFound = true;
                element = enumerator.Current;
            }
            enumerator.Dispose();
            return element;
        }
        
        public static T Single<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var wasFound = false;
            var element = default(T);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                {
                    if (wasFound)
                    {
                        enumerator.Dispose();
                        throw new InvalidOperationException("Sequence should contain only one element");
                    }

                    wasFound = true;
                    element = enumerator.Current;
                }
            }
            enumerator.Dispose();
            return element;
        }
		
        public static T Single<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) where TContext : struct
        {
            var wasFound = false;
            var element = default(T);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(context, enumerator.Current))
                {
                    if (wasFound)
                    {
                        enumerator.Dispose();
                        throw new InvalidOperationException("Sequence should contain only one element");
                    }

                    wasFound = true;
                    element = enumerator.Current;
                }
            }
            enumerator.Dispose();
            return element;
        }
        
        public static T SingleOrDefault<T>(this IPoolingEnumerable<T> source)
        {
            var wasFound = false;
            var element = default(T);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (wasFound)
                {
                    enumerator.Dispose();
                    return default;
                }

                wasFound = true;
                element = enumerator.Current;
            }
            enumerator.Dispose();
            return element;
        }
        
        public static T SingleOrDefault<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var wasFound = false;
            var element = default(T);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                {
                    if (wasFound)
                    {
                        enumerator.Dispose();
                        return default;
                    }

                    wasFound = true;
                    element = enumerator.Current;
                }
            }
            enumerator.Dispose();
            return element;
        }
		
        public static T SingleOrDefault<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) where TContext : struct
        {
            var wasFound = false;
            var element = default(T);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(context, enumerator.Current))
                {
                    if (wasFound)
                    {
                        enumerator.Dispose();
                        return default;
                    }

                    wasFound = true;
                    element = enumerator.Current;
                }
            }
            enumerator.Dispose();
            return element;
        }
    }
}