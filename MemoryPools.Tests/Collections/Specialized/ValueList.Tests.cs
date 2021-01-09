// using System;
// using NUnit.Framework;

namespace MemoryPools.Tests.Collections.Specialized
{
	// [TestFixture]
	// private class ValueListTests
	// {
	// 	[Test]
	// 	public void TestEmpty()
	// 	{
	// 		ValueList<int> list = default;
	//
	// 		Assert.AreEqual(0, list.Count);
	// 		Assert.AreEqual(false, list.IsReadOnly);
	// 		Assert.AreEqual(-1, list.IndexOf(1));
	// 	}
	//
	// 	[Test]
	// 	public void AllAdditions()
	// 	{
	// 		ValueList<int> list = default;
	// 		
	// 		for (int i = 0; i < ValueList<int>.Capacity; i++)
	// 		{
	// 			list.Add(i * 10);
	// 		}
	// 		
	// 		Assert.AreEqual(ValueList<int>.Capacity, list.Count);
	// 	}
	// 	
	// 	[Test]
	// 	public void SingleAddition()
	// 	{
	// 		ValueList<int> list = default;
	// 		list.Add(30);
	// 		
	// 		Assert.AreEqual(1, list.Count);
	// 		Assert.AreEqual(30, list[0]);
	// 	}
	//
	// 	[Test]
	// 	public void SingleAdditionAndRemove()
	// 	{
	// 		ValueList<int> list = default;
	// 		list.Add(30);
	// 		list.Remove(30);
	// 		
	// 		Assert.AreEqual(0, list.Count);
	// 		Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
	// 	}
	//
	// 	[Test]
	// 	public void SingleAdditionAndRemoveAt()
	// 	{
	// 		ValueList<int> list = default;
	// 		list.Add(30);
	// 		list.RemoveAt(0);
	// 		
	// 		Assert.AreEqual(0, list.Count);
	// 		Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
	// 	}
	//
	// 	[Test]
	// 	public void RemoveIfEmpty()
	// 	{
	// 		ValueList<int> list = default;
	//
	// 		Assert.AreEqual(0, list.Count);
	// 		Assert.AreEqual(false, list.Remove(29)); 
	// 	}
	// 	
	// 	[Test]
	// 	public void RemoveAtIfEmpty()
	// 	{
	// 		ValueList<int> list = default;
	//
	// 		Assert.AreEqual(0, list.Count);
	// 		Assert.Throws<IndexOutOfRangeException>(() => { list.RemoveAt(0); });
	// 	}
	//
	// 	[Test]
	// 	public void MakeListFromValueTuple()
	// 	{
	// 		ValueList<int> list = default;
	//
	// 		list.Add(10);
	// 		list.Add(20);
	// 		list.Add(30);
	// 		
	// 		Assert.AreEqual(3, list.Count);
	// 		Assert.AreEqual(10, list[0]);
	// 		Assert.AreEqual(20, list[1]);
	// 		Assert.AreEqual(30, list[2]);
	// 		
	// 		Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[3]; });
	// 	}
	//
	// 	[Test]
	// 	public void MakeListFromValueTupleAndGoBack()
	// 	{
	// 		ValueList<int> list = default;
	//
	// 		list.Add(10);
	// 		list.Add(20);
	// 		list.Add(30);
	// 		list.Remove(30);
	//
	// 		Assert.AreEqual(2, list.Count);
	// 		Assert.AreEqual(10, list[0]);
	// 		Assert.AreEqual(20, list[1]);
	// 		
	// 		Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[2]; });
	// 	}
	//
	// 	[Test]
	// 	public void MakeListFromValueTupleClearAndFillBack()
	// 	{
	// 		ValueList<int> list = default;
	//
	// 		list.Add(10);
	// 		list.Add(20);
	// 		list.Add(30);
	//
	// 		list.Clear();
	// 		
	// 		Assert.AreEqual(0, list.Count);
	// 		Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
	// 		
	// 		list.Add(10);
	// 		list.Add(20);
	// 		list.Add(30);
	//
	// 		Assert.AreEqual(3, list.Count);
	// 		Assert.AreEqual(10, list[0]);
	// 		Assert.AreEqual(20, list[1]);
	// 		Assert.AreEqual(30, list[2]);
	// 		
	// 		Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[3]; });
	// 	}
	// }
}