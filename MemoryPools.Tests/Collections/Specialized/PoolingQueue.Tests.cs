// using MemoryPools.Collections.Specialized;
// using NUnit.Framework;
//
// namespace MemoryPools.Tests.Collections.Specialized
// {
// 	public class PoolingQueueTests
// 	{
// 		
// 		[Test]
// 		public void TestEmpty()
// 		{
// 			using var list = new PoolingQueueRef<object>();
//
// 			Assert.AreEqual(0, list.Count);
// 			Assert.AreEqual(true, list.IsEmpty);
// 		}
//
// 		[Test]
// 		public void SingleAddition()
// 		{
// 			using var list = new PoolingQueueRef<object>();
// 			list.Enqueue(30);
//
// 			Assert.AreEqual(1, list.Count);
// 			Assert.AreEqual(30, list.Dequeue());
// 		}
// 		
// 		[Test]
// 		public void AllAdditions()
// 		{
// 			using var list = new PoolingQueueRef<object>();
// 			
// 			for (int i = 0; i < PoolsDefaults.DefaultPoolBucketSize; i++)
// 			{
// 				list.Enqueue(i * 10);
// 			}
// 			
// 			Assert.AreEqual(PoolsDefaults.DefaultPoolBucketSize, list.Count);
// 		}
//
// 		[Test]
// 		public void SingleAdditionAndRemove()
// 		{
// 			using var list = new PoolingQueueRef<object>();
// 			list.Enqueue(30);
// 			list.Enqueue(60);
// 			list.Dequeue();
// 			
// 			Assert.AreEqual(1, list.Count);
// 			Assert.AreEqual(60, list.Dequeue());
// 		}
// 	}
// }