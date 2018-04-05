namespace Example.Cache
{
    public class LRUCacheThreadNotSafe<TKey, TValue> : LRUCache<TKey, TValue>
    {
        public LRUCacheThreadNotSafe(int capacity) : base(capacity) { }

        public override void AddOrUpdate(TKey key, TValue value)
        {
            AddOrUpdateThreadNotSafe(key, value);
        }
    }
}