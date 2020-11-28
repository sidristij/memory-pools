﻿using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<(T, T)> Zip<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> second) => 
            ObjectsPool<ZipExprEnumerable<T>>.Get().Init(source, second);
    }
}