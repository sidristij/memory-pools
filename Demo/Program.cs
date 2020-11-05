using System;
using System.Linq;
using MemoryPools.Collections;
using MemoryPools.Collections.Linq;
using MemoryPools.Collections.Specialized;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new PoolingListVal<int>();
            var list2 = new PoolingListVal<int>();
            for (var i = 0; i < 10; i++)
            {
                list1.Add(i*2);
                if(i < 6) list2.Add(i*2+1);
            }

            // while (!Console.KeyAvailable)
            foreach (var tuple in list1.AsPooling().Union(list2.AsPooling()))
            {
                Console.WriteLine($"{tuple}");
            }
            // foreach (var num in list.AsPooling().Append(100).Prepend(100))
            // {
            //     Console.WriteLine(num);
            // }
            
            // Console.WriteLine(list.AsPooling().FirstOrDefault());
        }
    }
}