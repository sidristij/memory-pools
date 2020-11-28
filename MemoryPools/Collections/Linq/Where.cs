using System;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
	public static partial class PoolingEnumerable
	{
		public static IPoolingEnumerable<T> Where<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition) => 
			ObjectsPool<WhereExprEnumerable<T>>.Get().Init(source, condition);

		public static IPoolingEnumerable<T> Where<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition) => 
			ObjectsPool<WhereExprWithContextEnumerable<T, TContext>>.Get().Init(source, context, condition);
	}
}