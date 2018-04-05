using System;

namespace Example.Cache
{
    /// <summary>
    /// Thread safe LRU Cache
    /// </summary>
    public class ConcurrentLRUCache<TKey, TValue> : LRUCache<TKey, TValue>
    {
        private readonly Object _lockThis = new Object();

        public ConcurrentLRUCache(int capacity) : base(capacity) { }

        public override void AddOrUpdate(TKey key, TValue value)
        {
            lock (_lockThis)
            {
                base.AddOrUpdate(key, value);
            }
            
        }
    }
}