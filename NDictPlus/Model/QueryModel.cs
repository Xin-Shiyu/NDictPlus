﻿using Nativa;
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
        private Trie<string> myTrie;

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

        public QueryModel()
        {

        }
    }
}
