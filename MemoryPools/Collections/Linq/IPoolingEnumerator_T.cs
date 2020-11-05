namespace MemoryPools.Collections.Linq
{
    public interface IPoolingEnumerator<out T> : IPoolingEnumerator
    {
        // <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        new T Current { get; }
    }
}