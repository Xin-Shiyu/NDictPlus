using System;
using System.Collections;
using System.Collections.Generic;

namespace Nativa
{
    public class Trie<TValue> : IDictionary<string, TValue> where TValue : class
    {
        private IEnumerable<KeyValuePair<string, TValue>> EntriesFrom(KeyNode node)
        {
            var nodeStack = new Stack<KeyNode>();
            nodeStack.Push(node);

            while (nodeStack.Count != 0) // iterate through every node
            {
                var currentNode = nodeStack.Pop();
                if (currentNode.Value != null)
                {
                    yield return
                        new KeyValuePair<string, TValue>(
                            currentNode.Key,
                            currentNode.Value);

                }
                foreach (var childPair in currentNode.Children)
                {
                    nodeStack.Push(childPair.Value);
                }
            }
            yield break;
        }

        private class ReversedComparer : IComparer<char>
        {
            public int Compare(char x, char y)
            {
                return y.CompareTo(x);
            }
        }

        private static readonly ReversedComparer comparer = new ReversedComparer();

        private class KeyNode
        {
            public string Key;
            public TValue Value;
            public readonly SortedDictionary<char, KeyNode> Children =
                new SortedDictionary<char, KeyNode>(comparer);
        }

        private readonly KeyNode Root = new KeyNode();

        public ICollection<string> Keys => throw new NotSupportedException();

        public ICollection<TValue> Values => throw new NotSupportedException();

        public int Count { get; private set; } = 0;

        public bool IsReadOnly => false;

        public TValue this[string key]
        { 
            get
            {
                if (!TryGetValue(key, out var value))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }
            set
            {
                if (!FindEntry(key, doInsert: false, out var target))
                {
                    throw new KeyNotFoundException();
                }
                target.Key = key;
                target.Value = value;
            }
        }

        public void Add(string key, TValue value)
        {
            if (FindEntry(key, doInsert: true, out var target))
            {
                throw new ArgumentException("Key already exists");
            }
            target.Key = key;
            target.Value = value;
            Count += 1;
        }

        public bool ContainsKey(string key)
        {
            return FindEntry(key, doInsert: false, out _);
        }

        public bool Remove(string key)
        {
            var result = FindEntry(key, doInsert: false, out var target);
            target.Value = null;
            return result;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            var result = FindEntry(key, doInsert: false, out var target);
            value = target.Value;
            return result;
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Root.Children.Clear();
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            if (!TryGetValue(item.Key, out var value)) return false;

            if (value != item.Value) return false;

            return true;
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            if (!FindEntry(item.Key, doInsert: false, out var target)) return false;

            if (target.Value != item.Value) return false;

            target.Value = null;

            Count -= 1;

            return true;
        }

        public IEnumerable<KeyValuePair<string, TValue>> BeginningWith(string beginning)
        {
            var current = Root;

            foreach (var c in beginning)
            {
                if (!current.Children.TryGetValue(c, out var next)) yield break;
                current = next;
            }

            foreach (var entry in EntriesFrom(current)) yield return entry;
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return EntriesFrom(Root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private bool FindEntry(string key, bool doInsert, out KeyNode node)
        {
            node = default;

            var len = key.Length;

            if (len == 0) throw new ArgumentException();

            var current = Root;

            if (doInsert)
            {
                for (int i = 0; i < len; ++i)
                {
                    if (current.Children.TryGetValue(key[i], out var nextNode))
                    {
                        current = nextNode;
                    }
                    else
                    {
                        var newNode = new KeyNode();
                        current.Children.Add(key[i], newNode);
                        current = newNode;
                    }    
                }
            }
            else
            {
                for (int i = 0; i < len; ++i)
                {
                    if (!current.Children.TryGetValue(key[i], out current))
                    {
                        return false;
                    }
                }
            }

            node = current;

            return node.Value != null;
        }
    }
}
