using System.Buffers;
using System.Runtime.CompilerServices;

namespace MemoryPools.Memory
{
	public static class CountdownMemoryOwnerEx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMemoryOwner<T> AddOwner<T>(this IMemoryOwner<T> that)
		{
			(that as CountdownMemoryOwner<T>)?.AddOwner();
			return that;
		}
	}
}