using System.Collections.Generic;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static PoolingList<T> AsPoolingList<T>(this IEnumerable<T> source)
        {
            var collection = ObjectsPool<PoolingList<T>>.Get().Init();
            collection.AddRange(source);
            return collection;
        }

        public static PoolingList<T> AsPoolingList<T>(this IPoolingEnumerable<T> source)
        {
            var collection = ObjectsPool<PoolingList<T>>.Get().Init();
            collection.AddRange(source);
            return collection;
        }

        public static PoolingDictionary<TK, TV> AsPoolingDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> source)
        {
            var collection = ObjectsPool<PoolingDictionary<TK, TV>>.Get().Init();
            collection.AddRange(source);
            return collection;
        }

        public static PoolingDictionary<TK, TV> AsPoolingDictionary<TK, TV>(this IPoolingEnumerable<KeyValuePair<TK, TV>> source)
        {
            var collection = ObjectsPool<PoolingDictionary<TK, TV>>.Get().Init();
            collection.AddRange(source);
            return collection;
        }

        public static void AddRange<T>(this PoolingList<T> target, IEnumerable<T> src)
        {
            foreach (var item in src)
            {
                target.Add(item);
            }
        }

        public static void AddRange<T>(this PoolingList<T> target, IPoolingEnumerable<T> src)
        {
            foreach (var item in src)
            {
                target.Add(item);
            }
        }
        
        public static void AddRange<TK, TV>(this PoolingDictionary<TK, TV> target, IEnumerable<KeyValuePair<TK, TV>> src)
        {
            foreach (var item in src)
            {
                target.Add(item.Key, item.Value);
            }
        }
        
        public static void AddRange<TK, TV>(this PoolingDictionary<TK, TV> target, IPoolingEnumerable<KeyValuePair<TK, TV>> src)
        {
            foreach (var item in src)
            {
                target.Add(item.Key, item.Value);
            }
        }
        
        public static void AddRangeSafe<TK, TV>(this PoolingDictionary<TK, TV> target, IEnumerable<KeyValuePair<TK, TV>> src)
        {
            foreach (var item in src)
            {
                target[item.Key] = item.Value;
            }
        }
        
        public static void AddRangeSafe<TK, TV>(this PoolingDictionary<TK, TV> target, IPoolingEnumerable<KeyValuePair<TK, TV>> src)
        {
            foreach (var item in src)
            {
                target[item.Key] = item.Value;
            }
        }
    }
}