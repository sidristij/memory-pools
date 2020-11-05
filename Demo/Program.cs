using System;
using System.Linq;
using MemoryPools.Collections.Linq;
using MemoryPools.Collections.Specialized;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new PoolingListVal<int>();
            for (var i = 0; i < 1000; i++)
            {
                list.Add(i);
            }
            
            while (!Console.KeyAvailable)
            foreach (var num in list.AsPooling().Where(x=> (x & 1) == 1).Cast<object>().OfType<int>())
            {
                Console.WriteLine(num);
            }
        }
    }
}