using Nativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace NDictPlus.Model
{
    class QueryModel
    {
        private Trie<string> myTrie = new Trie<string>()
        {
            {"shit", "fuck"}
        };

        public class TrieQueryResult<T> 
            : IEnumerable<KeyValuePair<string, T>>, INotifyCollectionChanged
            where T : class
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private readonly Trie<T> source;

            private IEnumerator<KeyValuePair<string, T>> enumerator;

            public TrieQueryResult(Trie<T> source)
            {
                this.source = source;
                enumerator = source.GetEnumerator();
            }

            public void Query(string query)
            {
                enumerator = source.BeginningWith(query).GetEnumerator();
                CollectionChanged.Invoke(
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

        public TrieQueryResult<string> Result { get; private set; }

        public QueryModel()
        {
            Result = new TrieQueryResult<string>(myTrie);
            Random random = new Random();
            for (int i = 0; i < 2000; ++i)
            {
                string key = string.Empty;
                for (int j = 0; j < 20; ++j)
                {
                    key += new string[] { "", "k", "s", "t", "n", "h", "m", "y", "r", "w", "g", "z", "d", "p", "b" }[random.Next(0, 15)];
                    key += "aiueo"[random.Next(0, 5)].ToString();
                    key += "nm "[random.Next(0, 3)].ToString();
                }
                if (!myTrie.ContainsKey(key)) myTrie.Add(key, ((char)random.Next('一', '草')).ToString());
            }
        }

        private string queryWord;

        public string QueryWord
        {
            get => queryWord;

            set
            {
                queryWord = value;
                Result.Query(value);
            }
        }
    }
}
