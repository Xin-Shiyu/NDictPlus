using Nativa;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NDictPlus.Model
{
    class BookModel
    {
        private readonly Trie<DescriptionModel> myTrie;

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

        public TrieQueryModel<DescriptionModel> QueryModel { get; private set; }
        
        public int PhraseCount { get => myTrie.Count; }

        public void Create(string phrase)
        {
            myTrie.Add(phrase, new DescriptionModel());
        }

        public bool Exists(string phrase)
        {
            return myTrie.ContainsKey(phrase);
        }

        public DescriptionModel GetExactResult(string phrase)
        {
            if (myTrie.TryGetValue(phrase, out var res))
            {
                return res;
            }
            return null;
        }

        public BookModel(Trie<DescriptionModel> trie)
        {
            myTrie = trie;
            QueryModel = new TrieQueryModel<DescriptionModel>(trie);
        }
    }
}
