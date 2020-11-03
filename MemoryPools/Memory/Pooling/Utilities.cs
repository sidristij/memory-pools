namespace MemoryPools.Memory.Pooling
{
	internal static class Utilities
	{
		internal static int GetMaxSizeForBucket(int binIndex)
		{
			return 16 << binIndex;
		}

		internal static int GetBucket(int size)
		{
			size--;
			var length = 0;
			while (size >= 16)
			{
				length++;
				size = size >> 1;
			}

			return length;
		}
	}
}