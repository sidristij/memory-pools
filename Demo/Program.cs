using System;
using MemoryPools.Collections.Linq;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            while(!Console.KeyAvailable)
            foreach (var grp in PoolingEnumerable
                .Range(0, 100)
                .Where(x => x % 2 == 0)
                .Select(x => new Item { key = x,  value = x >> 2 })
                .GroupBy(x => x.key, (key, vals) => new Item { key = key, value = vals.First().value }))
            {
                // Console.WriteLine(grp.value);
            }
        }
    }

    struct Item
    {
        public int key;
        public int value;
    }
}