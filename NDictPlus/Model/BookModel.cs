using Nativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NDictPlus.Model
{
    class BookModel
    {
        private readonly Trie<DescriptionModel> baseTrie;

        public class TrieQueryModel<T>
            : IEnumerable<KeyValuePair<string, T>>, INotifyCollectionChanged
            where T : class
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private readonly Trie<T> source;

            private IEnumerator<KeyValuePair<string, T>> enumerator;

            public TrieQueryModel(Trie<T> source)
            {
                this.source = source;
                enumerator = source.GetEnumerator();
            }

            public void Query(string query)
            {
                enumerator = source.BeginningWith(query).GetEnumerator();
                CollectionChanged?.Invoke(
                    this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Reset));
            }

            public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
            {
                return enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerator;
            }
        }

        public TrieQueryModel<DescriptionModel> NewQueryModel()
            => new TrieQueryModel<DescriptionModel>(baseTrie);

        public int PhraseCount { get => baseTrie.Count; }

        public void Create(string phrase)
        {
            baseTrie.Add(phrase, new DescriptionModel());
        }

        public bool Exists(string phrase)
        {
            return baseTrie.ContainsKey(phrase);
        }

        public DescriptionModel GetExactResult(string phrase)
        {
            if (baseTrie.TryGetValue(phrase, out var res))
            {
                return res;
            }
            return null;
        }

        public BookModel(Trie<DescriptionModel> trie)
        {
            baseTrie = trie;
        }

        public BookModel() : this(new Trie<DescriptionModel>()) { }

        public Trie<DescriptionModel> GetTrie() => baseTrie;
        // I don't want it to be a property because it isn't a property
        // semantically
    }
}
