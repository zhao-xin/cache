using System;
using System.Threading;
using System.Threading.Tasks;
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

        //should pass
        [Test]
        public void SafeMultiThreadTest()
        {
            ICache<int, char> LRUCache = new LRUCache<int, char>(6);
            MultiThreadTest(LRUCache);
        }

        //should fail
        [Test]
        public void UnsafeMultiThreadTest()
        {
            ICache<int, char> LRUCache = new LRUCacheThreadNotSafe<int, char>(6);
            MultiThreadTest(LRUCache);
        }

        private void MultiThreadTest(ICache<int, char> LRUCache)
        {
            var thread1 = new Thread(() =>
            {
                LRUCache.AddOrUpdate(1, 'a');
                LRUCache.AddOrUpdate(2, 'b');
                LRUCache.AddOrUpdate(3, 'c');
            });
            var thread2 = new Thread(() =>
            {
                LRUCache.AddOrUpdate(11, 'A');
                LRUCache.AddOrUpdate(12, 'B');
                LRUCache.AddOrUpdate(13, 'C');
            });

            //Thread1 and Thread2 add data into Cache simultaneously.
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            //After Thread1 and Thread 2 finish, all data can be retrieved.
            char value;
            bool result;
            result = LRUCache.TryGetValue(1, out value);
            Assert.IsTrue(result, "key 1 should be in Cache");
            Assert.AreEqual(value, 'a', "the value of key 1 should be 'a'");
            result = LRUCache.TryGetValue(2, out value);
            Assert.IsTrue(result, "key 2 should be in Cache");
            Assert.AreEqual(value, 'b', "the value of key 2 should be 'b'");
            result = LRUCache.TryGetValue(3, out value);
            Assert.IsTrue(result, "key 3 should be in Cache");
            Assert.AreEqual(value, 'c', "the value of key 3 should be 'c'");
            result = LRUCache.TryGetValue(11, out value);
            Assert.IsTrue(result, "key 11 should be in Cache");
            Assert.AreEqual(value, 'A', "the value of key 11 should be 'A'");
            result = LRUCache.TryGetValue(12, out value);
            Assert.IsTrue(result, "key 12 should be in Cache");
            Assert.AreEqual(value, 'B', "the value of key 12 should be 'B'");
            result = LRUCache.TryGetValue(13, out value);
            Assert.IsTrue(result, "key 13 should be in Cache");
            Assert.AreEqual(value, 'C', "the value of key 13 should be 'C'");

            //Add new data to wash out old data 
            LRUCache.AddOrUpdate(21, 'a');
            LRUCache.AddOrUpdate(22, 'a');
            LRUCache.AddOrUpdate(23, 'a');
            LRUCache.AddOrUpdate(24, 'a');
            LRUCache.AddOrUpdate(25, 'a');
            LRUCache.AddOrUpdate(26, 'a');

            //Old data should not be in cache
            result = LRUCache.TryGetValue(1, out value);
            Assert.IsFalse(result, "key 1 should NOT be in Cache");
            result = LRUCache.TryGetValue(2, out value);
            Assert.IsFalse(result, "key 2 should NOT be in Cache");
            result = LRUCache.TryGetValue(3, out value);
            Assert.IsFalse(result, "key 3 should NOT be in Cache");
            result = LRUCache.TryGetValue(11, out value);
            Assert.IsFalse(result, "key 11 should NOT be in Cache");
            result = LRUCache.TryGetValue(12, out value);
            Assert.IsFalse(result, "key 12 should NOT be in Cache");
            result = LRUCache.TryGetValue(13, out value);
            Assert.IsFalse(result, "key 13 should NOT be in Cache");
        }
    }
}