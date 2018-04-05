# A C# implementation of in-memory least recently used cache management

#### Requirments:
- Least Recently Used (LRU) cache. Cache has finite capacity. When cache is full,  the least recently added/updated/retrieved item be evicted from the cache.
- All operations, including cache eviction, must have _O(1)_ time complexity.
- The cache must be thread-safe.

#### Solution:
Dictionary and double linked list are used. The header node in list is the most recently used one. The tail node in list is the least recently used one. There are in total three operations in Cache management:

- **Insert**: When Cache is not full, the new data item only needs to be inserted into list header. Time complexity is _O(1)_. 
- **Replacement**: When Cache is full, delete the tail node of list, and insert the new data item into the list header. Time complexity is _O(1)_.
- **Retrieve/Update**: Each time a data item is retrieved/updated, the node of this data item is moved to the head of list. Becasue Dictionary is based on Hash, the time complexity is _O(1)_.

To achieve **thread-safe**, a lock is used on Write operations including Insert, Replacement and Update.

#### Tests:
To show the difference between thread not safe and threa safe, two types of LRU Cache are implemented:
- _LRUCache_ is thread-unsafe.
- _ConcurrentLRUCache_ is thread-safe.

There are in total four tests:
- _SingleThreadLRUTCacheTest_: Test LRU logic for _LRUTCache_.
- _SingleThreadConcurrentLRUTCacheTest_: Test LRU logic for _ConcurrentLRUTCache_.
- _MultiThreadLRUTCacheTest_: Test thread-safe for _LRUTCache_. **Note: this test should fail because _LRUCache_ is thread-unsafe**
- _MultiThreadConcurrentLRUTCacheTest_: Test thread-safe for _ConcurrentLRUTCache_.


**Note:** in function _LRUCache.InsertToFront()_, there is a line about _thread.sleep_. The purpose of this line is to make a big chance of error due to thread-unsafe. Delete this line in real applications.

#### Test Coverage:
Reshaper indicates test coverage is 99%. 
The 1% uncovered code is about the part after _MultiThreadLRUTCacheTest_ fail.
See Tests.PNG
