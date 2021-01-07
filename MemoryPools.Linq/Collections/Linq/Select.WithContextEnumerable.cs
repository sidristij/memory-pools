
using System;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
	internal class SelectExprWithContextEnumerable<T, TR, TContext> : IPoolingEnumerable<TR>
	{
		private IPoolingEnumerable<T> _src;
		private Func<TContext, T, TR> _condition;
		private TContext _context;
		private int _count;

		public SelectExprWithContextEnumerable<T, TR, TContext> Init(IPoolingEnumerable<T> src, TContext context, Func<TContext, T, TR> condition)
		{
			_src = src;
			_count = 0;
			_context = context;
			_condition = condition;
			return this;
		}

		public IPoolingEnumerator<TR> GetEnumerator()
		{
			_count++;
			return ObjectsPool<SelectExprWithContextEnumerator>.Get().Init(this, _src.GetEnumerator(), _context, _condition);
		}

		private void Dispose()
		{
			if (_count == 0) return;
			_count--;
			
			if (_count == 0)
			{
				_src = default;
				_context = default;
				_condition = default;
				ObjectsPool<SelectExprWithContextEnumerable<T, TR, TContext>>.Return(this);
			}
		}

		internal class SelectExprWithContextEnumerator : IPoolingEnumerator<TR>
		{
			private TContext _context;
			private Func<TContext, T, TR> _condition;
			private IPoolingEnumerator<T> _src;
			private SelectExprWithContextEnumerable<T, TR, TContext> _parent;
    
			public SelectExprWithContextEnumerator Init(
				SelectExprWithContextEnumerable<T, TR, TContext> parent, 
				IPoolingEnumerator<T> src, 
				TContext context, 
				Func<TContext, T, TR> condition) 
			{
				_src = src;
				_parent = parent;
				_context = context;
				_condition = condition;
				return this;
			}
    			
			public bool MoveNext() => _src.MoveNext();

			public void Reset() => _src.Reset();
			object IPoolingEnumerator.Current => Current;

			public TR Current => _condition(_context, _src.Current);
    
			public void Dispose()
			{
				_parent?.Dispose();
				_parent = default;
				_src?.Dispose();
				_src = default;
				_context = default;
				ObjectsPool<SelectExprWithContextEnumerator>.Return(this);
			}
		}

		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}