using System.Linq;
using MemoryPools;
using MemoryPools.Collections.Specialized;
using NUnit.Framework;

namespace DevTools.Common.Tests.Memory
{
    [TestFixture]
    public class PoolTests
    {
        [Test]
        public void TestRegularObjectGet()
        {
            var inst = Pool.Get<Instance>().Init("test");
            Assert.NotNull(inst);
            Assert.AreEqual("test", inst.Property);
        }        
        
        [Test]
        public void TestGetReturnGetReturnsTheSame()
        {
            var inst = Pool.Get<Instance>().Init("test");
            Pool.Return(inst);
            var second = Pool.Get<Instance>();
            
            Assert.NotNull(second);
            Assert.AreEqual(inst, second);
        }
        
        [Test]
        public void TestFillBucketPlusOne()
        {
            var list = Enumerable
                .Range(0, PoolsDefaults.DefaultPoolBucketSize * PoolsDefaults.DefaultPoolBucketSize + 1)
                .Select((_) => Pool.Get<Instance>().Init("test"))
                .ToList();

            foreach (var instance in list)
            {
                Pool.Return(instance);
            }
            
            foreach (var _ in list)
            {
                var x = Pool.Get<Instance>();
                Assert.NotNull(x);
                Assert.AreEqual("test", x.Property);
            }
            
            var newinst = Pool.Get<Instance>();
            Assert.NotNull(newinst);
            Assert.AreNotEqual("test", newinst.Property);
        }

        private class Instance
        {
            public string Property { get; set; }
            
            public Instance Init(string prop)
            {
                Property = prop;
                return this;
            }
        }
    }
}