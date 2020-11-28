using System.Collections.Generic;
using MemoryPools.Collections.Linq;
using MemoryPools.Memory;

namespace MemoryPools.Collections
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
			return ObjectsPool<GenericPoolingEnumerable<T>>.Get().Init(source);
		}

		public static IEnumerable<T> AsEnumerable<T>(this IPoolingEnumerable<T> source)
		{
			return ObjectsPool<GenericEnumerable<T>>.Get().Init(source);
		}
	}
}