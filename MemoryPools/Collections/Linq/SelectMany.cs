using System;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<TR> SelectMany<T, TR>(
            this IPoolingEnumerable<T> source,
            Func<T, IPoolingEnumerable<TR>> mutator)
        {
            return ObjectsPool<SelectManyExprEnumerable<T, TR>>.Get().Init(source, mutator);
        }
	
        public static IPoolingEnumerable<TR> SelectMany<T, TR, TContext>(
            this IPoolingEnumerable<T> source,
            TContext context,
            Func<T, TContext, IPoolingEnumerable<TR>> mutator)
        {
            return ObjectsPool<SelectManyExprWithContextEnumerable<T, TR, TContext>>.Get().Init(source, mutator, context);
        }
    }
}