using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList
	{
		public static IPoolingEnumerable<T> AsSingleEnumerableList<T>(this IEnumerable<T> src)
		{
			var list = Pool.Get<PoolingList<T>>().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool.Get<EnumerableTyped<T>>().Init(list);
		}
		
		public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>(this IEnumerable<T> src) where T : class
		{
			var list = Pool.Get<PoolingListCanon<T>>().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool.Get<EnumerableShared<T>>().Init(list);
		}
		public static IPoolingEnumerable<T> AsSingleEnumerableList<T>(this IPoolingEnumerable<T> src)
		{
			var list = Pool.Get<PoolingList<T>>().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool.Get<EnumerableTyped<T>>().Init(list);
		}
		
		public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>(this IPoolingEnumerable<T> src) where T : class
		{
			var list = Pool.Get<PoolingListCanon<T>>().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool.Get<EnumerableShared<T>>().Init(list);
		}
	}
}