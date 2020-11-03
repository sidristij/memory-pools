using System;
using System.Collections.Generic;

namespace MemoryPools.Collections.Linq
{
	public interface IPoolingEnumerable<out T>
	{
		// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		IPoolingEnumerator<T> GetEnumerator();
	}

	public interface IPoolingEnumerator<out T> : IDisposable
	{
		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		bool MoveNext();

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		void Reset();
		
		// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		T Current { get; }
	}

	public static partial class EnumerableEx
	{
		public static IPoolingEnumerable<T> AsPooling<T>(this IEnumerable<T> source)
		{
			return Heap.Get<GenericPoolingEnumerable<T>>().Init(source);
		}

		public static IEnumerable<T> AsEnumerable<T>(this IPoolingEnumerable<T> source)
		{
			return Heap.Get<GenericEnumerable<T>>().Init(source);
		}
	}
}