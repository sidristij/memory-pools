using System;

namespace MemoryPools.Collections.Linq
{
	internal class WhereExprWithContextEnumerable<T, TContext> : IPoolingEnumerable<T> where TContext : struct
	{
		private int _count;
		private IPoolingEnumerable<T> _src;
		private Func<TContext, T, bool> _condition;
		private TContext _context;

		public WhereExprWithContextEnumerable<T, TContext> Init(IPoolingEnumerable<T> src, TContext context, Func<TContext, T, bool> condition)
		{
			_count = 0;
			_src = src;
			_context = context;
			_condition = condition;
			return this;
		}

		public IPoolingEnumerator<T> GetEnumerator()
		{
			_count++;
			return Pool.Get<WhereExprWithContextEnumerator>().Init(_src.GetEnumerator(), this, _context, _condition);
		}

		private void Dispose()
		{
			if (_count == 0) return;
			_count--;
			if (_count == 0)
			{
				(_condition, _context, _src) = (default, default, default);
				Pool.Return(this);
			}
		}

		internal class WhereExprWithContextEnumerator : IPoolingEnumerator<T>
		{
			private TContext _context;
			private Func<TContext, T, bool> _condition;
			private IPoolingEnumerator<T> _src;
			private WhereExprWithContextEnumerable<T, TContext> _parent;
    
			public WhereExprWithContextEnumerator Init(
				IPoolingEnumerator<T> src,
				WhereExprWithContextEnumerable<T, TContext> parent,
				TContext context,
				Func<TContext, T, bool> condition) 
			{
				_src = src;
				_parent = parent;
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

			object IPoolingEnumerator.Current => Current;

			public T Current => _src.Current;
    
			public void Dispose()
			{
				_parent?.Dispose();
				_parent = null;
				_src?.Dispose();
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