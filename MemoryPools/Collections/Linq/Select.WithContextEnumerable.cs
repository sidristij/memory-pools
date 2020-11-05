
using System;

namespace MemoryPools.Collections.Linq
{
	internal class SelectClauseWithContextEnumerable<T, TR, TContext> : IPoolingEnumerable<TR> where TContext : struct
	{
		private IPoolingEnumerable<T> _src;
		private Func<TContext, T, TR> _condition;
		private TContext _context;

		public SelectClauseWithContextEnumerable<T, TR, TContext> Init(IPoolingEnumerable<T> src, TContext context, Func<TContext, T, TR> condition)
		{
			_src = src;
			_context = context;
			_condition = condition;
			return this;
		}

		public IPoolingEnumerator<TR> GetEnumerator()
		{
			var (condition, context, src) = (_condition, _context, _src);
			(_condition, _context, _src) = (default, default, default);
			Pool.Return(this);
			return Pool.Get<SelectClauseWithContextEnumerator>().Init(src.GetEnumerator(), context, condition);
		}

		internal class SelectClauseWithContextEnumerator : IPoolingEnumerator<TR>
		{
			private TContext _context;
			private Func<TContext, T, TR> _condition;
			private IPoolingEnumerator<T> _src;
    
			public SelectClauseWithContextEnumerator Init(IPoolingEnumerator<T> src, TContext context, Func<TContext, T, TR> condition) 
			{
				_src = src;
				_context = context;
				_condition = condition;
				return this;
			}
    			
			public bool MoveNext() => _src.MoveNext();

			public void Reset() => _src.Reset();
			object IPoolingEnumerator.Current => Current;

			public TR Current =>  _condition(_context, _src.Current);
    
			public void Dispose()
			{
				_src.Dispose();
				_src = default;
				_context = default;
				Pool.Return(this);
			}
		}

		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}