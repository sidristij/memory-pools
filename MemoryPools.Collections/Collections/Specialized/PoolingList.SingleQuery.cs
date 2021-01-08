using System.Collections.Generic;

namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList
	{
		public static IPoolingEnumerable<T> AsSingleEnumerableList<T>(this IEnumerable<T> src)
		{
			var list = Pool<PoolingList<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool<EnumerableTyped<T>>.Get().Init(list);
		}
		
		public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>(this IEnumerable<T> src) where T : class
		{
			var list = Pool<PoolingListCanon<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool<EnumerableShared<T>>.Get().Init(list);
		}
		public static IPoolingEnumerable<T> AsSingleEnumerableList<T>(this IPoolingEnumerable<T> src)
		{
			var list = Pool<PoolingList<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool<EnumerableTyped<T>>.Get().Init(list);
		}
		
		public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>(this IPoolingEnumerable<T> src) where T : class
		{
			var list = Pool<PoolingListCanon<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool<EnumerableShared<T>>.Get().Init(list);
		}
	}
}