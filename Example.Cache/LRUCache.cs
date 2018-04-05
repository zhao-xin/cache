using System;
using System.Collections.Generic;

namespace Example.Cache
{
    public class LRUCache<TKey, TValue> : ICache<TKey, TValue>
    {
        //node in double linked List 
        internal class LRUCacheNode
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public LRUCacheNode Prev { get; set; }
            public LRUCacheNode Next { get; set; }

            public LRUCacheNode(){}
        };

        private Dictionary<TKey, LRUCacheNode> _dictionary = new Dictionary<TKey, LRUCacheNode>();
        private LRUCacheNode _head = new LRUCacheNode();
        private LRUCacheNode _tail = new LRUCacheNode();
        private readonly int _capacity = 0; //maximum number of elements permitted

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _head.Prev = null;
            _head.Next = _tail;
            _tail.Prev = _head;
            _tail.Next = null;
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key) == false) //key is not in cache
            {
                if (_dictionary.Count == _capacity) // cache is full
                    RemoveFromTail();

                LRUCacheNode node = new LRUCacheNode();
                node.Key = key;
                node.Value = value;
                _dictionary[key] = node;
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

        private void RemoveFromTail()
        {
            LRUCacheNode node = _tail.Prev;
            DetachNode(node);
            _dictionary.Remove(node.Key);
        }

        private void DetachNode(LRUCacheNode node)
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
        }


        private void InsertToFront(LRUCacheNode node)
        {
            node.Next = _head.Next;
            node.Prev = _head;
            _head.Next = node;
            node.Next.Prev = node;
        }
    }
}