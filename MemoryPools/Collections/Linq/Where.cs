using System;

namespace MemoryPools.Collections.Linq
{
	public static partial class EnumerableEx
	{
		public static IPoolingEnumerable<T> Where<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
		{
			return Pool.Get<WhereClauseEnumerable<T>>().Init(source, condition);
		}
		
		public static IPoolingEnumerable<T> Where<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) where TContext : struct
		{
			return Pool.Get<WhereClauseWithContextEnumerable<T, TContext>>().Init(source, context, condition);
		}
	}
}