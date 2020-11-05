using System.Collections.Generic;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Specialized
{
	public static partial class AsSingleQueryList
	{
		public static IEnumerable<T> AsSingleEnumerableRefList<T>(this IEnumerable<T> src) where T : class
		{
			var list = Pool.Get<PoolingListRef<T>>().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool.Get<EnumerableRef<T>>().Init(list);
		}
		
		public static IEnumerable<T> AsSingleEnumerableValList<T>(this IEnumerable<T> src) where T : struct
		{
			var list = Pool.Get<PoolingListVal<T>>().Init();
			foreach (var item in src)
			{
				list.Add(item);
			}
			return Pool.Get<EnumerableVal<T>>().Init(list);
		}
	}
}