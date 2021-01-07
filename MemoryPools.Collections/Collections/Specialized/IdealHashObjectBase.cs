namespace MemoryPools.Collections.Specialized
{
	public abstract class IdealHashObjectBase
	{ 
		internal int IdealHashCode { get; set; }

		public override int GetHashCode() => IdealHashCode;
	}
}