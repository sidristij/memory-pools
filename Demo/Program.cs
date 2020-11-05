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
            for (var i = 0; i < 10; i++)
            {
                list.Add(i);
            }
            
            // while (!Console.KeyAvailable)
            foreach (var num in list.AsPooling().Append(100).Prepend(100))
            {
                Console.WriteLine(num);
            }
        }
    }
}