using System;
using System.Collections.Generic;
using System.Threading;

namespace Example.Cache
{
    /// <summary>
    /// Least Recently Used (LRU) Cache management
    /// When the cache is constructed, it should take as an argument the maximum number of elements stored in the cache.
    /// When an item is added to the cache, a check should be run to see if the cache size exceeds the maximum number of elements permitted. 
    /// If this is the case, then the least recently added/updated/retrieved item should be evicted from the cache.
    /// All operations, including cache eviction, must have O(1) time complexity.
    /// </summary>
    public class LRUCache<TKey, TValue> : ICache<TKey, TValue>
    {
        //node in double linked List 
        protected internal class LRUCacheNode
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public LRUCacheNode Prev { get; set; }
            public LRUCacheNode Next { get; set; }

            public LRUCacheNode(){}
        };

        Dictionary<TKey, LRUCacheNode> _dictionary = new Dictionary<TKey, LRUCacheNode>();
        protected LRUCacheNode _head = new LRUCacheNode();
        protected LRUCacheNode _tail = new LRUCacheNode();
        protected readonly int _capacity = 0; //maximum number of elements permitted
        private readonly Object _lockThis = new Object();

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _head.Prev = null;
            _head.Next = _tail;
            _tail.Prev = _head;
            _tail.Next = null;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_dictionary.ContainsKey(key) == false) //key is not in cache
            {
                value = _head.Value;
                return false;
            }
            else
            {
                LRUCacheNode node = _dictionary[key];
                DetachNode(node);
                InsertToFront(node);
                value = node.Value;
                return true;
            }
        }

        virtual public void AddOrUpdate(TKey key, TValue value)
        {
            lock (_lockThis) 
            {
                AddOrUpdateThreadNotSafe(key, value);
            }
        }

        protected void AddOrUpdateThreadNotSafe(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key) == false) //key is not in cache
            {
                if (_dictionary.Count == _capacity) // cache is full
                    RemoveFromTail();

                LRUCacheNode node = new LRUCacheNode();
                node.Key = key;
                node.Value = value;
                _dictionary.Add(key, node);
                InsertToFront(node);
            }
            else
            {
                LRUCacheNode node = _dictionary[key];
                DetachNode(node);
                node.Value = value;
                InsertToFront(node);
            }
        }

        protected void RemoveFromTail()
        {
            LRUCacheNode node = _tail.Prev;
            DetachNode(node);
            _dictionary.Remove(node.Key);
        }

        protected void DetachNode(LRUCacheNode node)
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
        }


        protected void InsertToFront(LRUCacheNode node)
        {
            node.Next = _head.Next;
            Thread.Sleep(100); //keep this line to make a big change of error due to unsafe multiple threads. todo: delete this line in real applications.
            node.Prev = _head;
            _head.Next = node;
            node.Next.Prev = node;
        }
    }
}