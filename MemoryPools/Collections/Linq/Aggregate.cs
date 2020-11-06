using System;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static TSource Aggregate<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            var result = enumerator.Current;
            while (enumerator.MoveNext())
            {
                result = func(result, enumerator.Current);
            }

            return result;
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IPoolingEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var result = seed;
            foreach (var element in source)
            {
                result = func(result, element);
            }

            return result;
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IPoolingEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var result = seed;
            foreach (var element in source)
            {
                result = func(result, element);
            }

            return resultSelector(result);
        }
    }
}