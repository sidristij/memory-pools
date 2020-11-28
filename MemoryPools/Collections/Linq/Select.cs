using System;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
	public static partial class PoolingEnumerable
	{
		public static IPoolingEnumerable<TR> Select<T, TR>(this IPoolingEnumerable<T> source, Func<T, TR> mutator)
		{
			return ObjectsPool<SelectExprEnumerable<T, TR>>.Get().Init(source, mutator);
		}
		
		public static IPoolingEnumerable<TR> Select<T, TR, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, TR> mutator)
		{
			return ObjectsPool<SelectExprWithContextEnumerable<T, TR, TContext>>.Get().Init(source, context, mutator);
		}
	}
}