using System;

namespace MemoryPools.Collections.Linq
{
	public static partial class PoolingEnumerable
	{
		public static IPoolingEnumerable<T> Where<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition) => 
			Pool<WhereExprEnumerable<T>>.Get().Init(source, condition);

		public static IPoolingEnumerable<T> Where<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) => 
			Pool<WhereExprWithContextEnumerable<T, TContext>>.Get().Init(source, context, condition);
	}
}