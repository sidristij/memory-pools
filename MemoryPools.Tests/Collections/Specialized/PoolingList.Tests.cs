﻿using System;
using MemoryPools.Collections.Specialized;
using NUnit.Framework;

namespace MemoryPools.Tests.Collections.Specialized
{
	public class PoolingListTests
	{
		[Test]
		public void TestEmpty()
		{
			using (var list = new PoolingListCanon<object>())
			{
				Assert.AreEqual(0, list.Count);
				Assert.AreEqual(false, list.IsReadOnly);
				Assert.AreEqual(-1, list.IndexOf(1));
			}
		}

		[Test]
		public void SingleAddition()
		{
			using (var list = new PoolingListCanon<object>())
			{
				list.Add(30);

				Assert.AreEqual(1, list.Count);
				Assert.AreEqual(30, list[0]);
			}
		}
		
		[Test]
		public void AllAdditions()
		{
			using (var list = new PoolingListCanon<object>())
			{
				for (int i = 0; i < LocalList<int>.LocalStoreCapacity; i++)
				{
					list.Add(i * 10);
				}

				Assert.AreEqual(LocalList<int>.LocalStoreCapacity, list.Count);
			}
		}

		[Test]
		public void SingleAdditionAndRemove()
		{
			using (var list = new PoolingListCanon<object>())
			{
				list.Add(30);
				list.Remove(30);

				Assert.AreEqual(0, list.Count);
				Assert.Throws<IndexOutOfRangeException>(() =>
				{
					var _ = list[0];
				});
			}
		}

		[Test]
		public void SingleAdditionAndRemoveAt()
		{
			using var list = new PoolingListCanon<object>();
			list.Add(30);
			list.RemoveAt(0);
			
			Assert.AreEqual(0, list.Count);
			Assert.Throws<IndexOutOfRangeException>(() => { var _ = list[0]; });
		}

		[Test]
		public void RemoveIfEmpty()
		{
			using var list = new PoolingListCanon<object>();

			Assert.AreEqual(0, list.Count);
			Assert.AreEqual(false, list.Remove(29)); 
		}
		
		[Test]
		public void RemoveAtIfEmpty()
		{
			using var list = new PoolingListCanon<object>();

			Assert.AreEqual(0, list.Count);
			Assert.Throws<IndexOutOfRangeException>(() => { list.RemoveAt(0); });
		}

		[Test]
		public void MakeListFromValueTuple()
		{
			using var list = new PoolingListCanon<object>();
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
			using var list = new PoolingListCanon<object>();

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
			using var list = new PoolingListCanon<object>();

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