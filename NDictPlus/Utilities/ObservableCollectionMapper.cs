using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NDictPlus.Utilities
{
    public static class ObservableCollectionMapper
    {
        public static ObservableCollectionMapper<TSource, TTarget>
            Map<TSource, TTarget>(
            IEnumerable<TSource> sourceCollection,
            Func<TSource, TTarget> mapper)
            => new ObservableCollectionMapper<TSource, TTarget>(sourceCollection, mapper);
    }

    public class ObservableCollectionMapper<TSource, TTarget> 
        : INotifyCollectionChanged, IEnumerable<TTarget>, IDisposable
    {
        readonly Func<TSource, TTarget> mapper;
        readonly IEnumerable<TSource> sourceCollection;

        public 
            ObservableCollectionMapper
            (IEnumerable<TSource> sourceCollection, Func<TSource, TTarget> mapper)
        {
            this.sourceCollection = sourceCollection;
            this.mapper = mapper;
            System.Diagnostics.Debug.WriteLine($"{typeof(TSource)} to {typeof(TTarget)}");
            System.Diagnostics.Debug.WriteLine(sourceCollection);
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
        {
            System.Diagnostics.Debug.WriteLine($"Source Collection {sender} Changed!");
            CollectionChanged.Invoke
                (this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
            // inefficient but currently useful
            // to be optimized later on

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<TTarget> GetEnumerator()
        {
            System.Diagnostics.Debug.WriteLine($"gets enumerator of {this.GetType()}");
            return sourceCollection.Select(mapper).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            if (sourceCollection is INotifyCollectionChanged notifiable)
            {
                notifiable.CollectionChanged -= OnSourceCollectionChanged;
            }
        }
        // disposing is very very very important!!!!!!!!
    }
}
