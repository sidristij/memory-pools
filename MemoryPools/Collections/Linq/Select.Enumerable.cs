using System;

namespace MemoryPools.Collections.Linq
{
	internal class SelectClauseEnumerable<T, TR> : IPoolingEnumerable<TR>
	{
		private IPoolingEnumerable<T> _src;
		private Func<T, TR> _mutator;

		public SelectClauseEnumerable<T, TR> Init(IPoolingEnumerable<T> src, Func<T, TR> mutator)
		{
			_src = src;
			_mutator = mutator;
			return this;
		}

		public IPoolingEnumerator<TR> GetEnumerator()
		{
			var (condition, src) = (_condition: _mutator, _src);
			(_mutator, _src) = (default, default);
			Pool.Return(this);
			return Pool.Get<SelectClauseEnumerator>().Init(src.GetEnumerator(), condition);
		}

		internal class SelectClauseEnumerator : IPoolingEnumerator<TR>
		{
			private Func<T, TR> _mutator;
			private IPoolingEnumerator<T> _src;

			public SelectClauseEnumerator Init(IPoolingEnumerator<T> src, Func<T, TR> mutator) 
			{
				_src = src;
				_mutator = mutator;
				return this;
			}

			public bool MoveNext()
			{
				return _src.MoveNext();
			}

			public void Reset()
			{
				_src.Reset();
			}

			public TR Current => _mutator( _src.Current);

			public void Dispose()
			{
				_src.Dispose();
				_src = default;
				Pool.Return(this);
			}
		}
	}
}