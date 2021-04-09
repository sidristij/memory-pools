using System;
using System.Buffers;
using System.Linq;
using MemoryPools.Collections.Linq;

namespace Demo
{
    static class Program
    {
        static void Main()
        {
            // SimplePooling();
            // TestRegularLinq();
            TestPoolingLinq();
        }

        static void SimplePooling()
        {
            var item = Pool<PoolingItem>.Get().Init(123);

            Pool<PoolingItem>.Return(item);
        }

        static void ComplexPooling()
        {
            var buf = Pool.GetBuffer<int>(1024);
            
            using (var item = Pool<ComplexPoolingItem>.Get().Init(buf))
            {
                // ...
            }
            
            buf.Dispose();
        }

        static void TestRegularLinq()
        {
            while(!Console.KeyAvailable)
            {
                foreach (var grp in Enumerable
                    .Range(0, 100)
                    .Where(x => x % 2 == 0)
                    .Select(x => new Context { key = x,  value = x >> 2 })
                    .GroupBy(x => x.key, (key, vals) => new Context { key = key, value = vals.First().value }))
                {
                    // Console.WriteLine(grp.value);
                }
            }
        }

        static void TestPoolingLinq()
        {
            while(!Console.KeyAvailable)
            {
                foreach (var grp in PoolingEnumerable
                    .Range(0, 100)
                    .Where(x => x % 2 == 0)
                    .Select(x => new Context { key = x,  value = x >> 2 })
                    .GroupBy(x => x.key, (key, vals) => new Context { key = key, value = vals.First().value }))
                {
                    // Console.WriteLine(grp.value);
                }
            }
        }
    }

    class PoolingItem : IDisposable
    {
        private int _x;

        public int Value => _x;

        public PoolingItem Init(int val)
        {
            _x = val;
            return this;
        }

        public void Dispose()
        {
            _x = default;
            // ...
        }
    }

    class ComplexPoolingItem : IDisposable
    {
        private IMemoryOwner<int> _buf;

        public ComplexPoolingItem Init(IMemoryOwner<int> source)
        {
            _buf = source;
            _buf.AddOwner();
            return this;
        }

        public void Dispose()
        {
            _buf?.Dispose();
            _buf = null;
        }
    }
    
    struct Context
    {
        public int key;
        public int value;
    }
}