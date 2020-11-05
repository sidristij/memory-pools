namespace MemoryPools.Collections.Linq
{
    public interface IPoolingEnumerable<out T> : IPoolingEnumerable
    {
        // <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        new IPoolingEnumerator<T> GetEnumerator();
    }
}