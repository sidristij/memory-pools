using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static int Sum(this IPoolingEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int sum = 0;
            checked {
                foreach (var v in source) sum += v;
            }
            return sum;
        }
 
        public static int? Sum(this IPoolingEnumerable<int?> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            int sum = 0;
            checked {
                foreach (var v in source) {
                    if (v != null) sum += v.GetValueOrDefault();
                }
            }
            return sum;
        }
 
        public static long Sum(this IPoolingEnumerable<long> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            long sum = 0;
            checked {
                foreach (long v in source) sum += v;
            }
            return sum;
        }
 
        public static long? Sum(this IPoolingEnumerable<long?> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            long sum = 0;
            checked {
                foreach (var v in source) {
                    if (v != null) sum += v.GetValueOrDefault();
                }
            }
            return sum;
        }
 
        public static float Sum(this IPoolingEnumerable<float> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            double sum = 0;
            foreach (var v in source) sum += v;
            return (float)sum;
        }
 
        public static float? Sum(this IPoolingEnumerable<float?> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            double sum = 0;
            foreach (var v in source) {
                if (v != null) sum += v.GetValueOrDefault();
            }
            return (float)sum;
        }
 
        public static double Sum(this IPoolingEnumerable<double> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            double sum = 0;
            foreach (var v in source) sum += v;
            return sum;
        }
 
        public static double? Sum(this IPoolingEnumerable<double?> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            double sum = 0;
            foreach (var v in source) {
                if (v != null) sum += v.GetValueOrDefault();
            }
            return sum;
        }
 
        public static decimal Sum(this IPoolingEnumerable<decimal> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            decimal sum = 0;
            foreach (var v in source) sum += v;
            return sum;
        }
 
        public static decimal? Sum(this IPoolingEnumerable<decimal?> source) {
            if (source == null)  throw new ArgumentNullException(nameof(source));
            decimal sum = 0;
            foreach (var v in source) {
                if (v != null) sum += v.GetValueOrDefault();
            }
            return sum;
        }
 
        public static int Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, int> selector) => 
            Sum(source.Select(selector));

        public static int? Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, int?> selector) => 
            Sum(source.Select(selector));

        public static long Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, long> selector) => 
            Sum(source.Select(selector));

        public static long? Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, long?> selector) => 
            Sum(source.Select(selector));

        public static float Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, float> selector) => 
            Sum(source.Select(selector));

        public static float? Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, float?> selector) => 
            Sum(source.Select(selector));

        public static double Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, double> selector) => 
            Sum(source.Select(selector));

        public static double? Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, double?> selector) => 
            Sum(source.Select(selector));

        public static decimal Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, decimal> selector) => 
            Sum(source.Select(selector));

        public static decimal? Sum<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, decimal?> selector) => 
            Sum(source.Select(selector));
    }
}