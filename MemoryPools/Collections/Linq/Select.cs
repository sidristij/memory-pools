using System;

namespace MemoryPools.Collections.Linq
{
	public static partial class EnumerableEx
	{
		public static IPoolingEnumerable<TR> Select<T, TR>(this IPoolingEnumerable<T> source, Func<T, TR> mutator)
		{
			return Pool.Get<SelectClauseEnumerable<T, TR>>().Init(source, mutator);
		}
		
		public static IPoolingEnumerable<TR> Select<T, TR, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, TR> mutator) where TContext : struct
		{
			return Pool.Get<SelectClauseWithContextEnumerable<T, TR, TContext>>().Init(source, context, mutator);
		}
	}
}