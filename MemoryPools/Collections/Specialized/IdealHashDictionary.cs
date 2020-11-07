using System;

namespace MemoryPools.Collections.Specialized
{
	/// <summary>
	/// Represents ideal dictionary with extra fast access to its items. Items should inherit IdealHashObjectBase to be
	/// able to set hashcode.
	/// </summary>
	/// <typeparam name="TK">Key of dictionary</typeparam>
	/// <typeparam name="TV">Corresponding Value</typeparam>
	public class IdealHashDictionary<TK, TV> : 
		IDisposable
		where TK : IdealHashObjectBase
		where TV : class
	{
		readonly PoolingListCanon<TV> _list = Pool.Get<PoolingListCanon<TV>>().Init();
		readonly PoolingQueue<int> _freeNodes = new PoolingQueueVal<int>();
  
		public TV this[TK key]
		{
			get
			{
				var hc = key.IdealHashCode;
				if(hc >= _list.Count)
					throw new ArgumentOutOfRangeException(nameof(key));
				return _list[hc];
			}
		}
  
		public void Add(TK key, TV value)
		{
			var index = AcquireHashCode(value);
			key.IdealHashCode = index;
		}
  
		public bool Remove(TK key)
		{
			var index = key.IdealHashCode;
			_freeNodes.Enqueue(index);
			_list[index] = default;
			return true;
		}

		private int AcquireHashCode(TV value)
		{
			if(_freeNodes.Count > 0)
			{
				return _freeNodes.Dequeue();
			}
			_list.Add(value);
			return _list.Count - 1;
		}

		public void Dispose()
		{
			_list.Clear();
			_freeNodes.Clear();
		}
	}
}