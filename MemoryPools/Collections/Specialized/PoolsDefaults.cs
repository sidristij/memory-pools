namespace MemoryPools.Collections.Specialized
{
	public static class PoolsDefaults
	{
		public const int DefaultPoolBucketDegree = 7;
		public const int DefaultPoolBucketSize = 1 << DefaultPoolBucketDegree;
		public const int DefaultPoolBucketMask = DefaultPoolBucketSize - 1;
	}
}