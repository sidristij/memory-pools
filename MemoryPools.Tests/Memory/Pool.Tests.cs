using System.Linq;
using MemoryPools.Collections.Specialized;
using MemoryPools.Memory;
using NUnit.Framework;

namespace DevTools.Common.Tests.Memory
{
    [TestFixture]
    public class PoolTests
    {
        [Test]
        public void TestRegularObjectGet()
        {
            var inst = ObjectsPool<Instance>.Get().Init("test");
            Assert.NotNull(inst);
            Assert.AreEqual("test", inst.Property);
        }        
        
        [Test]
        public void TestGetReturnGetReturnsTheSame()
        {
            var inst = ObjectsPool<Instance>.Get().Init("test");
            ObjectsPool<Instance>.Return(inst);
            var second = ObjectsPool<Instance>.Get();
            
            Assert.NotNull(second);
            Assert.AreEqual(inst, second);
        }
        
        [Test]
        public void TestFillBucketPlusOne()
        {
            var list = Enumerable
                .Range(0, PoolsDefaults.DefaultPoolBucketSize * PoolsDefaults.DefaultPoolBucketSize + 1)
                .Select((_) => ObjectsPool<Instance>.Get().Init("test"))
                .ToList();

            foreach (var instance in list)
            {
                ObjectsPool<Instance>.Return(instance);
            }
            
            foreach (var _ in list)
            {
                var x = ObjectsPool<Instance>.Get();
                Assert.NotNull(x);
                Assert.AreEqual("test", x.Property);
            }
            
            var newinst = ObjectsPool<Instance>.Get();
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