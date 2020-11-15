using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace NDictPlus.Utilities
{
    public class ObservableDictionary<TKey, TValue>
        : INotifyCollectionChanged, IDictionary<TKey, TValue>
    {
        readonly IDictionary<TKey, TValue> dictionary;
        
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary = null)
        {
            if (dictionary == null)
            {
                this.dictionary = new Dictionary<TKey, TValue>();
            }
        }

        public TValue this[TKey key]
        { 
            get => dictionary[key];
            set
            {
                dictionary[key] = value;
                RaiseCollectionChanged(NotifyCollectionChangedAction.Replace);
            }
        }

        public ICollection<TKey> Keys => dictionary.Keys;

        public ICollection<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => dictionary.IsReadOnly;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        void RaiseCollectionChanged
            (NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Reset)
        {
            CollectionChanged?
                .Invoke(this, new NotifyCollectionChangedEventArgs(action));
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            RaiseCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item);
            RaiseCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public void Clear()
        {
            dictionary.Clear();
            RaiseCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            var res = dictionary.Remove(key);
            RaiseCollectionChanged(NotifyCollectionChangedAction.Remove);
            return res;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var res = dictionary.Remove(item);
            RaiseCollectionChanged(NotifyCollectionChangedAction.Remove);
            return res;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }
    }
}
