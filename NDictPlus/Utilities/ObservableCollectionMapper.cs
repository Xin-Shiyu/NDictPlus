using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.Net.Sockets;

namespace NDictPlus.Utilities
{
    public class ObservableCollectionMapper<TSource, TTarget> 
        : INotifyCollectionChanged, IEnumerable<TTarget>
    {
        readonly Func<TSource, TTarget> mapper;
        readonly IEnumerable<TSource> sourceCollection;

        public 
            ObservableCollectionMapper
            (IEnumerable<TSource> sourceCollection, Func<TSource, TTarget> mapper)
        {
            this.sourceCollection = sourceCollection;
            this.mapper = mapper;
            if (sourceCollection is INotifyCollectionChanged notifiable)
            {
                notifiable.CollectionChanged += OnSourceCollectionChanged;
            }
            else
            {
                throw new ArgumentException("sourceCollection");
            }
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => CollectionChanged(sender, e);

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<TTarget> GetEnumerator() => sourceCollection.Select(mapper).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
