using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList
	{
		public static IPoolingEnumerable<T> AsSingleEnumerableList<T>(this IEnumerable<T> src)
		{
			var list = ObjectsPool<PoolingList<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return ObjectsPool<EnumerableTyped<T>>.Get().Init(list);
		}
		
		public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>(this IEnumerable<T> src) where T : class
		{
			var list = ObjectsPool<PoolingListCanon<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return ObjectsPool<EnumerableShared<T>>.Get().Init(list);
		}
		public static IPoolingEnumerable<T> AsSingleEnumerableList<T>(this IPoolingEnumerable<T> src)
		{
			var list = ObjectsPool<PoolingList<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return ObjectsPool<EnumerableTyped<T>>.Get().Init(list);
		}
		
		public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>(this IPoolingEnumerable<T> src) where T : class
		{
			var list = ObjectsPool<PoolingListCanon<T>>.Get().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return ObjectsPool<EnumerableShared<T>>.Get().Init(list);
		}
	}
}