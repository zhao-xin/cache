# A C# implementation of in-memory least recently used cache management

**Requirments:**
1. Least Recently Used (LRU) cache. Cache has finite capacity. When cache is full,  the least recently added/updated/retrieved item be evicted from the cache.
2. All operations, including cache eviction, must have _O(1)_ time complexity.
3. The cache must be thread-safe.

**Solution:**
Dictionary and double linked list are used. The header node in list is the most recently used one. The tail node in list is the least recently used one. There are in total three operations in Cache management:

1. Insert: When Cache is not full, the new data item only needs to be inserted into list header. Time complexity is _O(1)_. 
2. Replacement: When Cache is full, delete the tail node of list, and insert the new data item into the list header. Time complexity is _O(1)_.
3. Retrieve/Update: Each time a data item is retrieved/updated, the node of this data item is moved to the head of list. Becasue Dictionary is based on Hash, the time complexity is _O(1)_.

To achieve thread safe, a lock is used on Write operations including Insert, Replacement and Update.

**Tests:**
There are in-total three tests:
1. SingleThreadTest: Test LRU cache management.
2. SafeMultiThreadTest: Test thread safe.
3. UnsafeMultiThreadTest: Show errors if the write operations are not within a lock section. A separate Cache implementation which is thread-not-safe (LRUCacheThreadNotSafe) is added for comparison. **Note: this test should fail because it is a test on LRUCacheThreadNotSafe**

**Note:** in function LRUCache.InsertToFront(), there is a line about thread.sleep. The purpose of this line is to make a big change of error due to unsafe multiple threads. Delete this line in real applications.

**Test Coverage:**
Reshaper indicates test coverage is 99%. 
See Tests.PNG
