using System;

namespace MemoryPools.Collections.Linq
{
	internal class SelectExprEnumerable<T, TR> : IPoolingEnumerable<TR>
	{
		private IPoolingEnumerable<T> _src;
		private Func<T, TR> _mutator;

		public SelectExprEnumerable<T, TR> Init(IPoolingEnumerable<T> src, Func<T, TR> mutator)
		{
			_src = src;
			_mutator = mutator;
			return this;
		}

		public IPoolingEnumerator<TR> GetEnumerator()
		{
			var (mutator, src) = (_mutator, _src);
			(_mutator, _src) = (default, default);
			Pool.Return(this);
			return Pool.Get<SelectExprEnumerator>().Init(src.GetEnumerator(), mutator);
		}

		internal class SelectExprEnumerator : IPoolingEnumerator<TR>
		{
			private Func<T, TR> _mutator;
			private IPoolingEnumerator<T> _src;

			public SelectExprEnumerator Init(IPoolingEnumerator<T> src, Func<T, TR> mutator) 
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

			object IPoolingEnumerator.Current => Current;

			public TR Current => _mutator( _src.Current);

			public void Dispose()
			{
				_src.Dispose();
				_src = default;
				Pool.Return(this);
			}
		}

		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}