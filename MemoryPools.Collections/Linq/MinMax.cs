using System;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static int Min(this IPoolingEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int value = 0;
            bool hasValue = false;
            foreach (int x in source) {
                if (hasValue) {
                    if (x < value) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static int? Min(this IPoolingEnumerable<int?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int? value = null;
            foreach (int? x in source) {
                if (value == null || x < value)
                    value = x;
            }
            return value;
        }
 
        public static long Min(this IPoolingEnumerable<long> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            long value = 0;
            bool hasValue = false;
            foreach (long x in source) {
                if (hasValue) {
                    if (x < value) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static long? Min(this IPoolingEnumerable<long?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            long? value = null;
            foreach (long? x in source) {
                if (value == null || x < value) value = x;
            }
            return value;
        }
 
        public static float Min(this IPoolingEnumerable<float> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            float value = 0;
            bool hasValue = false;
            foreach (float x in source) {
                if (hasValue) {
                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    if (x < value || System.Single.IsNaN(x)) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static float? Min(this IPoolingEnumerable<float?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            float? value = null;
            foreach (float? x in source) {
                if (x == null) continue;
                if (value == null || x < value || System.Single.IsNaN((float)x)) value = x;
            }
            return value;
        }
 
        public static double Min(this IPoolingEnumerable<double> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            double value = 0;
            bool hasValue = false;
            foreach (double x in source) {
                if (hasValue) {
                    if (x < value || Double.IsNaN(x)) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static double? Min(this IPoolingEnumerable<double?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            double? value = null;
            foreach (double? x in source) {
                if (x == null) continue;
                if (value == null || x < value || Double.IsNaN((double)x)) value = x;
            }
            return value;
        }
 
        public static decimal Min(this IPoolingEnumerable<decimal> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            decimal value = 0;
            bool hasValue = false;
            foreach (decimal x in source) {
                if (hasValue) {
                    if (x < value) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static decimal? Min(this IPoolingEnumerable<decimal?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            decimal? value = null;
            foreach (decimal? x in source) {
                if (value == null || x < value) value = x;
            }
            return value;
        }
 
        public static TSource Min<TSource>(this IPoolingEnumerable<TSource> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var comparer = Comparer<TSource>.Default;
            var value = default(TSource);
            if (value == null) {
                foreach (var x in source) {
                    if (x != null && (value == null || comparer.Compare(x, value) < 0))
                        value = x;
                }
                return value;
            }

            bool hasValue = false;
            foreach (TSource x in source) {
                if (hasValue) {
                    if (comparer.Compare(x, value) < 0)
                        value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static int Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, int> selector) {
            return source.Select(selector).Min();
        }
 
        public static int? Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, int?> selector) {
            return source.Select(selector).Min();
        }
 
        public static long Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, long> selector) {
            return source.Select(selector).Min();
        }
 
        public static long? Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, long?> selector) {
            return source.Select(selector).Min();
        }
 
        public static float Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, float> selector) {
            return source.Select(selector).Min();
        }
 
        public static float? Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, float?> selector) {
            return source.Select(selector).Min();
        }
 
        public static double Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, double> selector) {
            return source.Select(selector).Min();
        }
 
        public static double? Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, double?> selector) {
            return source.Select(selector).Min();
        }
 
        public static decimal Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, decimal> selector) {
            return source.Select(selector).Min();
        }
 
        public static decimal? Min<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, decimal?> selector) {
            return source.Select(selector).Min();
        }
 
        public static TResult Min<TSource, TResult>(this IPoolingEnumerable<TSource> source, Func<TSource, TResult> selector) {
            return source.Select(selector).Min();
        }
 
        public static int Max(this IPoolingEnumerable<int> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int value = 0;
            bool hasValue = false;
            foreach (int x in source) {
                if (hasValue) {
                    if (x > value) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static int? Max(this IPoolingEnumerable<int?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int? value = null;
            foreach (int? x in source) {
                if (value == null || x > value) value = x;
            }
            return value;
        }
 
        public static long Max(this IPoolingEnumerable<long> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            long value = 0;
            bool hasValue = false;
            foreach (long x in source) {
                if (hasValue) {
                    if (x > value) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static long? Max(this IPoolingEnumerable<long?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            long? value = null;
            foreach (long? x in source) {
                if (value == null || x > value) value = x;
            }
            return value;
        }
 
        public static double Max(this IPoolingEnumerable<double> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            double value = 0;
            bool hasValue = false;
            foreach (double x in source) {
                if (hasValue) {
                    if (x > value || Double.IsNaN(value)) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static double? Max(this IPoolingEnumerable<double?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            double? value = null;
            foreach (double? x in source) {
                if (x == null) continue;
                if (value == null || x > value || Double.IsNaN((double)value)) value = x;
            }
            return value;
        }
 
        public static float Max(this IPoolingEnumerable<float> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            float value = 0;
            bool hasValue = false;
            foreach (float x in source) {
                if (hasValue) {
                    if (x > value || Double.IsNaN(value)) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static float? Max(this IPoolingEnumerable<float?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            float? value = null;
            foreach (float? x in source) {
                if (x == null) continue;
                if (value == null || x > value || System.Single.IsNaN((float)value)) value = x;
            }
            return value;
        }
 
        public static decimal Max(this IPoolingEnumerable<decimal> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            decimal value = 0;
            bool hasValue = false;
            foreach (decimal x in source) {
                if (hasValue) {
                    if (x > value) value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static decimal? Max(this IPoolingEnumerable<decimal?> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            decimal? value = null;
            foreach (decimal? x in source) {
                if (value == null || x > value) value = x;
            }
            return value;
        }
 
        public static TSource Max<TSource>(this IPoolingEnumerable<TSource> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Comparer<TSource> comparer = Comparer<TSource>.Default;
            TSource value = default;
            if (value == null) {
                foreach (TSource x in source) {
                    if (x != null && (value == null || comparer.Compare(x, value) > 0))
                        value = x;
                }
                return value;
            }

            bool hasValue = false;
            foreach (TSource x in source) {
                if (hasValue) {
                    if (comparer.Compare(x, value) > 0)
                        value = x;
                }
                else {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new InvalidOperationException("Sequence contains no elements");
        }
 
        public static int Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, int> selector) => 
            Max(source.Select(selector));

        public static int? Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, int?> selector) => 
            Max(source.Select(selector));

        public static long Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, long> selector) => 
            Max(source.Select(selector));

        public static long? Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, long?> selector) => 
            Max(source.Select(selector));

        public static float Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, float> selector) => 
            Max(source.Select(selector));

        public static float? Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, float?> selector) => 
            Max(source.Select(selector));

        public static double Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, double> selector) => 
            Max(source.Select(selector));

        public static double? Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, double?> selector) => Max(source.Select(selector));

        public static decimal Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, decimal> selector) => 
            source.Select(selector).Max();

        public static decimal? Max<TSource>(this IPoolingEnumerable<TSource> source, Func<TSource, decimal?> selector) => 
            source.Select(selector).Max();

        public static TResult Max<TSource, TResult>(this IPoolingEnumerable<TSource> source, Func<TSource, TResult> selector) => 
            source.Select(selector).Max();
    }
}