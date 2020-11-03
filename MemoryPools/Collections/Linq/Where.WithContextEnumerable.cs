using System;

namespace MemoryPools.Collections.Linq
{
	internal class WhereClauseWithContextEnumerable<T, TContext> : IPoolingEnumerable<T> where TContext : struct
	{
		private IPoolingEnumerable<T> _src;
		private Func<TContext, T, bool> _condition;
		private TContext _context;

		public WhereClauseWithContextEnumerable<T, TContext> Init(IPoolingEnumerable<T> src, TContext context, Func<TContext, T, bool> condition)
		{
			_src = src;
			_context = context;
			_condition = condition;
			return this;
		}

		public IPoolingEnumerator<T> GetEnumerator()
		{
			var (condition, context, src) = (_condition, _context, _src);
			(_condition, _context, _src) = (default, default, default);
			Heap.Return(this);
			return Heap.Get<WhereClauseWithContextEnumerator>().Init(src.GetEnumerator(), context, condition);
		}

		internal class WhereClauseWithContextEnumerator : IPoolingEnumerator<T>
		{
			private TContext _context;
			private Func<TContext, T, bool> _condition;
			private IPoolingEnumerator<T> _src;
    
			public WhereClauseWithContextEnumerator Init(IPoolingEnumerator<T> src, TContext context, Func<TContext, T, bool> condition) 
			{
				_src = src;
				_context = context;
				_condition = condition;
				return this;
			}
    			
			public bool MoveNext()
			{
				do
				{
					var next = _src.MoveNext();
					if (!next) return false;
				} while (!_condition(_context, _src.Current));

				return true;
			}
    
			public void Reset()
			{
				_src.Reset();
			}

			public T Current => _src.Current;
    
			public void Dispose()
			{
				_src.Dispose();
				_src = default;
				_context = default;
				Heap.Return(this);
			}
		}
	}
}