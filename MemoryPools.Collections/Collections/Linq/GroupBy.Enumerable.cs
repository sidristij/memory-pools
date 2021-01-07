using System;
using System.Collections.Generic;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;

namespace MemoryPools.Collections.Linq
{
    internal sealed class GroupedEnumerable<TSource, TKey, TElement> : IPoolingEnumerable<IPoolingGrouping<TKey, TElement>>
    {
        private IPoolingEnumerable<TSource> _source;
        private Func<TSource, TKey> _keySelector;
        private Func<TSource, TElement> _elementSelector;
        private IEqualityComparer<TKey> _comparer;
        private int _count;

        public GroupedEnumerable<TSource, TKey, TElement> Init(
            IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _elementSelector = elementSelector ?? throw new ArgumentNullException(nameof(elementSelector));
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<IPoolingGrouping<TKey, TElement>> GetEnumerator()
        {
            var tmpDict = ObjectsPool<PoolingDictionary<TKey, PoolingGrouping>>.Get().Init(0, _comparer);
            
            PoolingGrouping grp;
            foreach (var item in _source)
            {
                var key = _keySelector(item);
                if (!tmpDict.TryGetValue(key, out grp))
                {
                    tmpDict[key] = grp = ObjectsPool<PoolingGrouping>.Get().Init(key);
                }

                grp.InternalList.Add(_elementSelector(item));
            }

            _count++;
            return ObjectsPool<PoolingGroupingEnumerator>.Get().Init(this, tmpDict);
        }

        private void Dispose()
        {
            if (_count == 0) return;
            _count--;
            
            if (_count == 0)
            {
                _comparer = default;
                _elementSelector = default;
                _keySelector = default;
                ObjectsPool<GroupedEnumerable<TSource, TKey, TElement>>.Return(this);
            }
        }
        
        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    
        internal class PoolingGroupingEnumerator : IPoolingEnumerator<IPoolingGrouping<TKey, TElement>>
        {
            private PoolingDictionary<TKey, PoolingGrouping> _src;
            private GroupedEnumerable<TSource, TKey, TElement> _parent;
            private IPoolingEnumerator<KeyValuePair<TKey, PoolingGrouping>> _enumerator;
            
            public PoolingGroupingEnumerator Init(
                GroupedEnumerable<TSource, TKey, TElement> parent,
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
                    ObjectsPool<PoolingGrouping>.Return(grouping.Value);
                }
                
                // cleanup collection
                _src?.Dispose();
                ObjectsPool<PoolingDictionary<TKey, PoolingGrouping>>.Return(_src);
                _src = default;
                
                _enumerator?.Dispose();
                _enumerator = default;
                
                _parent?.Dispose();
                _parent = default;
                
                ObjectsPool<PoolingGroupingEnumerator>.Return(this);
            }

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();

            public IPoolingGrouping<TKey, TElement> Current => _enumerator.Current.Value;

            object IPoolingEnumerator.Current => Current;
        }

        internal class PoolingGrouping : IPoolingGrouping<TKey, TElement>, IDisposable
        {
            private PoolingList<TElement> _elements;

            public PoolingGrouping Init(TKey key)
            {
                _elements = ObjectsPool<PoolingList<TElement>>.Get().Init();
                Key = key;
                return this;
            }

            internal PoolingList<TElement> InternalList => _elements;

            public IPoolingEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();

            IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

            public TKey Key { get; private set; }

            public void Dispose()
            {
                _elements?.Dispose();
                ObjectsPool<PoolingList<TElement>>.Return(_elements);
                _elements = null;
                
                Key = default;
            }
        }
    }
}