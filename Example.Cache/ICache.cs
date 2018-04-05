﻿namespace Example.Cache
{
    public interface ICache<TKey, TValue>
    {
        /// <summary>
        /// Adds the value to the cache against the specified key.
        /// If the key already exists, its value is updated.
        /// </summary>
        void AddOrUpdate(TKey key, TValue value);
        /// <summary>
        /// Attempts to gets the value from the cache against the specified key
        /// and returns true if the key existed in the cache.
        /// </summary>
        bool TryGetValue(TKey key, out TValue value);
    }
}