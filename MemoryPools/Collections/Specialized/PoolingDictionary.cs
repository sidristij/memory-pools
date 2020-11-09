using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MemoryPools.Collections.Specialized.Helpers;

namespace MemoryPools.Collections.Specialized
{
    /// <summary>
    /// Pooling dictionary contains two PoolingLists and should be disposed at end of life.
    /// Disallowed: removing elements and getting non-generic IEnumerable.
    /// When get IEnumerable[TKey, TValue], you need to dispose it (foreach Expr do it automatically).
    /// You can safe add any count of elements to dispose them: all collections stores data in 128-sized chunks.
    /// These chunks are reusable btw all Pooling* collections. All operations have O(1) complexity.
    /// </summary>
    public partial class PoolingDictionary<TKey, TValue> : 
        IDictionary<TKey, TValue>, 
        IReadOnlyDictionary<TKey, TValue>,
        IPoolingEnumerable<KeyValuePair<TKey, TValue>>,
        IDisposable
    {
        [DebuggerDisplay("Key: {key}, Value: {value}")]
        private struct Entry {
            public int hashCode;    // Lower 31 bits of hash code, -1 if unused
            public int next;        // Index of next entry, -1 if last
            public TKey key;        // Key of entry
            public TValue value;    // Value of entry
        }

        private IEqualityComparer<TKey> _comparer;
        private PoolingList<int> _buckets;
        private PoolingList<Entry> _entries;
        private int _freeList;
        private int _version;
        private int _freeCount;
        private int _count;
        private int _complexity;
        private bool _refType;
        private ICollection<TKey> _keys;
        private ICollection<TValue> _values;

        public PoolingDictionary() => Init(0);

        public PoolingDictionary<TKey, TValue> Init(int capacity, IEqualityComparer<TKey> comparer = default)
        {
            _refType = typeof(TKey).IsClass;
            var size = HashHelpers.GetPrime(capacity);
            _buckets = Pool.Get<PoolingList<int>>().Init();
            for (var i = 0; i < size; i++)
            {
                _buckets.Add(-1);
            }
            _entries = Pool.Get<PoolingList<Entry>>().Init();
            _freeList = -1;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            return this;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var i = FindEntry(key);
            if (i >= 0)
            {
                value = _entries[i].value;
                return true;
            }

            value = default;
            return false;
        }

        public TValue this[TKey key] 
        {
             get {
                 var i = FindEntry(key);
                 if (i >= 0) return _entries[i].value;
                 throw new KeyNotFoundException();
             }
             set => Insert(key, value, false);
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public ICollection<TKey> Keys => throw new NotImplementedException(); // _keys ??= ObjectsPool<KeysCollection>.Get().Init(this);

        public ICollection<TValue> Values => throw new NotImplementedException(); // _values ??= ObjectsPool<ValuesCollection>.Get().Init(this);

        private int FindEntry(TKey key) 
        {
            if( key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_buckets == null) return -1;
            var hashCode = key.GetHashCode() & 0x7FFFFFFF;
            for (var i = _buckets[hashCode % _buckets.Count]; i >= 0; i = _entries[i].next) 
            {
                if (_entries[i].hashCode == hashCode && _comparer.Equals(_entries[i].key, key)) return i;
            }
            return -1;
        }

        public int Complexity => _complexity;

        public void Add(TKey key, TValue value) => Insert(key, value, true);
        
        public bool ContainsKey(TKey key) => FindEntry(key) >= 0;

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        private void Insert(TKey key, TValue value, bool add) {
        
            if( _refType && key == null ) {
                throw new ArgumentNullException(nameof(key));
            }
 
            if (_buckets == null) Init(PoolsDefaults.DefaultPoolBucketSize);
            var hashCode = key.GetHashCode() & 0x7FFFFFFF;
            var targetBucket = hashCode % _buckets.Count;
            var complexity = 0;
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].next) 
            {
                if (_entries[i].hashCode == hashCode && _comparer.Equals(_entries[i].key, key)) 
                {
                    if (add) { 
                        throw new ArgumentException("Duplicating key found in dictionary");
                    }

                    var entrym = _entries[i];
                    entrym.value = value;
                    _entries[i] = entrym;
                    
                    unchecked
                    {
                        _version++;
                    }

                    return;
                }

                complexity++;
            }
            int index;
            if (_freeCount > 0) {
                index = _freeList;
                _freeList = _entries[index].next;
                _freeCount--;
            }
            else {
                if (_count == _entries.Count)
                {
                    Resize();
                    targetBucket = hashCode % _buckets.Count;
                }
                index = _count;
                _count++;
            }

            var entry = _entries[index];
            entry.hashCode = hashCode;
            entry.next = _buckets[targetBucket];
            entry.key = key;
            entry.value = value;
            _entries[index] = entry;
            _buckets[targetBucket] = index;
            
            unchecked
            {
                _version++;
            }

            _complexity = Math.Max(_complexity, complexity);
        }
        
        private void Resize() {
            Resize(HashHelpers.ExpandPrime(_count), false);
        }
 
        private void Resize(int newSize, bool forceNewHashCodes) {
            while(_buckets.Count < newSize) _buckets.Add(-1);
            while(_entries.Count < newSize) _entries.Add(default);
            
            if(forceNewHashCodes) 
            {
                for (var i = 0; i < _count; i++)
                {
                    if(_entries[i].hashCode != -1)
                    {
                        var entry = _entries[i];
                        entry.hashCode = _entries[i].key.GetHashCode() & 0x7FFFFFFF;
                        _entries[i] = entry;
                    }
                }
            }
            
            for (var i = 0; i < _count; i++)
            {
                if (_entries[i].hashCode < 0) continue;
                
                var bucket = _entries[i].hashCode % newSize;
                var entry = _entries[i];
                entry.next = _buckets[bucket];
                _entries[i] = entry;
                _buckets[bucket] = i;
            }
        }

        public void Dispose()
        {
            unchecked
            {
                _version++;
            }
            
            _buckets?.Dispose();
            Pool.Return(_buckets);

            _entries?.Dispose();
            Pool.Return(_entries);

            _buckets = default;
            _entries = default;
            _comparer = default;
            _complexity = _count = _version = _freeCount = _freeList = default;
        }

        public void Add(KeyValuePair<TKey, TValue> item) => 
            Insert(item.Key, item.Value, true);

        public void Clear()
        {
            _buckets.Clear();
            _entries.Clear();
            _complexity = 0;
            _count = _freeList = _freeCount = 0;
            
            unchecked
            {
                _version++;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var keyHash = item.Key.GetHashCode() & 0x7FFFFFFF;
            for (var i = 0; i < _entries.Count; i++)
            {
                if(_entries[i].hashCode == keyHash && _comparer.Equals(_entries[i].key, item.Key) && 
                   _entries[i].value.Equals(item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex < _entries.Count)
            {
                throw new IndexOutOfRangeException("Dictionary size bigger than array");
            }
            
            for (var i = 0; i < _entries.Count; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(_entries[i].key, _entries[i].value);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public int Count => _count;
        public bool IsReadOnly => false;

        internal class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IPoolingEnumerator<KeyValuePair<TKey, TValue>>
        {
            private PoolingDictionary<TKey, TValue> _src;
            private int _pos;
            private int _ver;
            
            public Enumerator Init(PoolingDictionary<TKey, TValue> src)
            {
                _pos = -1;
                _src = src;
                _ver = _src._version;
                return this;
            }
            
            public bool MoveNext()
            {
                if (_pos >= _src.Count) return false;
                if (_ver != _src._version)
                {
                    throw new InvalidOperationException("Version of collection was changed while enumeration");
                }
                _pos++;
                return _pos < _src._count;
            }

            public void Reset()
            {
                _ver = _src._version;
                _pos = -1;
            }

            object IPoolingEnumerator.Current => Current;

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    if (_ver != _src._version)
                    {
                        throw new InvalidOperationException("Version of collection was changed while enumeration");
                    }
                    return new KeyValuePair<TKey, TValue>(_src._entries[_pos].key, _src._entries[_pos].value);
                }
            }

            object IEnumerator.Current => throw new InvalidOperationException("Boxing disallowed");

            public void Dispose()
            {
                Pool.Return(this);
            }
        }

        public IPoolingEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            Pool.Get<Enumerator>().Init(this);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => 
            (IEnumerator<KeyValuePair<TKey, TValue>>)GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)GetEnumerator();


        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}