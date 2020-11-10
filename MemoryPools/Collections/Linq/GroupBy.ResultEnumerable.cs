using System;
using System.Collections.Generic;
using MemoryPools.Collections.Specialized;

namespace MemoryPools.Collections.Linq
{
    internal sealed class GroupedResultEnumerable<TSource, TKey, TResult> : IPoolingEnumerable<TResult>
    {
        private IPoolingEnumerable<TSource> _source;
        private Func<TSource, TKey> _keySelector;
        private Func<TKey, IPoolingEnumerable<TSource>, TResult> _resultSelector;
        private IEqualityComparer<TKey> _comparer;
        private int _count;

        public GroupedResultEnumerable<TSource, TKey, TResult> Init(
            IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IPoolingEnumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _resultSelector = resultSelector ?? throw new ArgumentNullException(nameof(resultSelector));
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<TResult> GetEnumerator()
        {
            var tmpDict = Pool.Get<PoolingDictionary<TKey, PoolingGrouping>>().Init(0, _comparer);
            
            PoolingGrouping grp;
            foreach (var item in _source)
            {
                var key = _keySelector(item);
                if (!tmpDict.TryGetValue(key, out grp))
                {
                    tmpDict[key] = grp = Pool.Get<PoolingGrouping>().Init(key);
                }

                grp.InternalList.Add(item);
            }

            _count++;
            return Pool.Get<GroupedResultEnumerator>().Init(this, tmpDict);
        }

        private void Dispose()
        {
            if (_count == 0) return;
            _count--;
            
            if (_count == 0)
            {
                _comparer = default;
                _resultSelector = default;
                _keySelector = default;
                Pool.Return(this);
            }
        }
        
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    
        internal class GroupedResultEnumerator : IPoolingEnumerator<TResult>
        {
            private PoolingDictionary<TKey, PoolingGrouping> _src;
            private GroupedResultEnumerable<TSource, TKey, TResult> _parent;
            private IPoolingEnumerator<KeyValuePair<TKey, PoolingGrouping>> _enumerator;
            
            public GroupedResultEnumerator Init(
                GroupedResultEnumerable<TSource, TKey, TResult> parent,
                PoolingDictionary<TKey, PoolingGrouping> src)
            {
                _src = src;
                _parent = parent;
                _enumerator = _src.GetEnumerator();
                return this;
            }

            public void Dispose()
            {
                // Cleanup contents
                foreach (var grouping in _src)
                {
                    grouping.Value.Dispose();
                    Pool.Return(grouping.Value);
                }
                
                // cleanup collection
                _src?.Dispose();
                Pool.Return(_src);
                _src = default;
                
                _enumerator?.Dispose();
                _enumerator = default;
                
                _parent?.Dispose();
                _parent = default;
                
                Pool.Return(this);
            }

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();

            public TResult Current => _parent._resultSelector(_enumerator.Current.Key, _enumerator.Current.Value.InternalList);

            object IPoolingEnumerator.Current => Current;
        }

        internal class PoolingGrouping : IPoolingGrouping<TKey, TSource>, IDisposable
        {
            private PoolingList<TSource> _elements;

            public PoolingGrouping Init(TKey key)
            {
                _elements = Pool.Get<PoolingList<TSource>>().Init();
                Key = key;
                return this;
            }

            internal PoolingList<TSource> InternalList => _elements;

            public IPoolingEnumerator<TSource> GetEnumerator() => _elements.GetEnumerator();

            IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

            public TKey Key { get; private set; }

            public void Dispose()
            {
                _elements?.Dispose();
                Pool.Return(_elements);
                _elements = null;
                
                Key = default;
            }
        }
    }
}