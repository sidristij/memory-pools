using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
	
	public interface IPoolingEnumerable
	{
		// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		IPoolingEnumerator GetEnumerator();
	}	
	
	public static partial class EnumerableEx
	{
		public static IPoolingEnumerable<T> AsPooling<T>(this IEnumerable<T> source)
		{
			return Pool.Get<GenericPoolingEnumerable<T>>().Init(source);
		}

		public static IEnumerable<T> AsEnumerable<T>(this IPoolingEnumerable<T> source)
		{
			return Pool.Get<GenericEnumerable<T>>().Init(source);
		}
	}
}