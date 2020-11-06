﻿using System;

namespace MemoryPools.Collections.Linq
{
    public static partial class EnumerableEx
    {
        public static IPoolingEnumerable<TR> SelectMany<T, TR>(
            this IPoolingEnumerable<T> source,
            Func<T, IPoolingEnumerable<TR>> mutator)
        {
            return Pool.Get<SelectManyExprEnumerable<T, TR>>().Init(source, mutator);
        }
	
        // public static IPoolingEnumerable<TR> SelectMany<T, TR, TContext>(
        //     this IPoolingEnumerable<T> source,
        //     TContext context,
        //     Func<TContext, T, IPoolingEnumerable<TR>> mutator) where TContext : struct
        // {
        //     return Pool.Get<SelectManyExprWithContextEnumerable<T, TR, TContext>>().Init(source, context, mutator);
        // }
    }
}