using System;
using System.Collections;

namespace MemoryPools
{
	internal struct Hasher : IEnumerable
	{
		public int? Hash { get; private set; }

		public void Add<T>(T value)
		{
			var hash = typeof(T).IsValueType
				? value.GetHashCode() 
				: value?.GetHashCode();

			AddImpl(hash);
		}

		public void Add<T>(T? value) where T : struct
		{
			AddImpl(value?.GetHashCode());
		}

		private void AddImpl(int? value)
		{
			Hash = Combine(Hash, value);
		}

		public override int GetHashCode()
		{
			return Hash ?? 17;
		}

		public static implicit operator int(Hasher h)
		{
			return h.GetHashCode();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public static int Combine(int? left, int? right)
		{
			unchecked
			{
				return 37 * (left ?? 17) + (right ?? 0);
			}
		}

		public Hasher Combine(int? value)
		{
			return new Hasher {Hash = Combine(Hash, value)};
		}
	}
}