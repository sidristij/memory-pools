using System;
using MemoryPools.Collections.Specialized;
using NUnit.Framework;

namespace MemoryPools.Tests.Collections.Specialized
{
	[TestFixture]
	public class LocalListTests
	{
		[Test]
		public void TestEmpty()
		{
			LocalList<int> list = default;

			Assert.AreEqual(0, list.Count);
			Assert.AreEqual(false, list.IsReadOnly);
			Assert.AreEqual(-1, list.IndexOf(1));
		}

		[Test]
		public void SingleAddition()
		{
			LocalList<int> list = default;
			list.Add(30);
			
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(30, list[0]);
		}
		
		[Test]
		public void AllAdditions()
		{
			LocalList<int> list = default;
			
			for (int i = 0; i < LocalList<int>.LocalStoreCapacity; i++)
			{
				list.Add(i * 10);
			}
			
			Assert.AreEqual(list[0], 0);
			Assert.AreEqual(list[1], 10);
			Assert.AreEqual(LocalList<int>.LocalStoreCapacity, list.Count);
		}

		[Test]
		public void SingleAdditionAndRemove()
		{
			LocalList<int> list = default;
			list.Add(30);
			list.Remove(30);
			
			Assert.AreEqual(0, list.Count);
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
		}
		
		[Test]
		public void TwoAdditionsAndRemove()
		{
			LocalList<int> list = default;
			list.Add(30);
			list.Add(30);
			list.Remove(30);
			
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(30, list[0]);
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[1]; });
		}
		
		[Test]
		public void ThreeAdditionsAndRemove()
		{
			LocalList<int> list = default;
			list.Add(10);
			list.Add(20);
			list.Add(30);
			
			list.Remove(30);
			
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[2]; });
		}

		[Test]
		public void SingleAdditionAndRemoveAt()
		{
			LocalList<int> list = default;
			list.Add(30);
			list.RemoveAt(0);
			
			Assert.AreEqual(0, list.Count);
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
		}

		[Test]
		public void RemoveIfEmpty()
		{
			LocalList<int> list = default;

			Assert.AreEqual(0, list.Count);
			Assert.AreEqual(false, list.Remove(29)); 
		}
		
		[Test]
		public void RemoveAtIfEmpty()
		{
			LocalList<int> list = default;

			Assert.AreEqual(0, list.Count);
			Assert.Throws<IndexOutOfRangeException>(() => { list.RemoveAt(0); });
		}

		[Test]
		public void MakeListFromValueTuple()
		{
			LocalList<int> list = default;

			list.Add(10);
			list.Add(20);
			list.Add(30);
			
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
			Assert.AreEqual(30, list[2]);
			
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[3]; });
		}

		[Test]
		public void MakeListFromValueTupleAndGoBack()
		{
			LocalList<int> list = default;

			list.Add(10);
			list.Add(20);
			list.Add(30);
			list.Remove(30);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
			
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[2]; });
		}

		[Test]
		public void MakeListFromValueTupleClearAndFillBack()
		{
			LocalList<int> list = default;

			list.Add(10);
			list.Add(20);
			list.Add(30);

			list.Clear();
			
			Assert.AreEqual(0, list.Count);
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
			
			list.Add(10);
			list.Add(20);
			list.Add(30);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
			Assert.AreEqual(30, list[2]);
			
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[3]; });
		}
	}
}