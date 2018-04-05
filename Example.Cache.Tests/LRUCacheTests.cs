using NUnit.Framework;

namespace Example.Cache.Tests
{
    [TestFixture]
    public class LRUCacheTests
    {
        [Test]
        public void SingleThreadTest()
        {
            ICache<int, char> LRUCache = new LRUCache<int, char>(5);
            char value;
            bool result;
            LRUCache.AddOrUpdate(1, 'a');
            LRUCache.AddOrUpdate(2, 'b');
            LRUCache.AddOrUpdate(3, 'c');
            LRUCache.AddOrUpdate(4, 'd');
            LRUCache.AddOrUpdate(5, 'e');
            result = LRUCache.TryGetValue(1, out value);
            Assert.IsTrue(result, "key 1 should be in Cache");
            Assert.AreEqual(value, 'a', "the value of key 1 should be 'a'");
            LRUCache.AddOrUpdate(6, 'e');
            result = LRUCache.TryGetValue(2, out value);
            Assert.IsFalse(result, "key 2 should be the least recently used one as key 1 has been retrieved.");
            LRUCache.AddOrUpdate(3, 'x');
            LRUCache.AddOrUpdate(7, 'f');
            result = LRUCache.TryGetValue(4, out value);
            Assert.IsFalse(result, "key 4 should be the least recently used one as key 3 has been updated.");
            LRUCache.TryGetValue(3, out value);
            Assert.AreEqual(value, 'x', "the value of key 1 should be 'x'");
        }
    }
}