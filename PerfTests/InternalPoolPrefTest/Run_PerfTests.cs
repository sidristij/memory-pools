using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using MemoryPools.Memory;

namespace InternalPoolPrefTest
{
	class Program
	{
		public static void Main(string[] args) => BenchmarkRunner.Run<Run_PerfTests>();
	}
	
	[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
	public class Run_PerfTests
	{
		[Benchmark(Description = "ArrayPool (ThreadStatic, Stack)", OperationsPerInvoke = 100)]
		public void TestInternalArrayPool_ThreadAwareQueue()
		{
			for (var i = 0; i < 100; i++) ObjectsPool<PoolItem>.Return(ObjectsPool<PoolItem>.Get());
		}

		class PoolItem
		{
			public int x;
		}
	}
}