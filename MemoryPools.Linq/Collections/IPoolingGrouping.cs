namespace MemoryPools.Collections
{
    public interface IPoolingGrouping<out TKey, out TElement> : IPoolingEnumerable<TElement>
    {
        TKey Key { get; }
    }
}