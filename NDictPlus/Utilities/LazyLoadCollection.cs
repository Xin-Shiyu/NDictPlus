using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NDictPlus.Utilities
{
    public static class LazyLoadCollection
    {
        public static LazyLoadCollection<T> From<T>(IEnumerable<T> underlying, int eachTimeCount = 10)
            => new LazyLoadCollection<T>(underlying, eachTimeCount);
    }

    public class LazyLoadCollection<T> : ObservableCollection<T>, IDisposable
    {
        readonly IEnumerable<T> underlying;
        readonly int eachTimeCount;

        IEnumerator<T> enumerator;
        bool finished = false;

        public void LoadMore()
        {
            if (finished) return;
            int loaded = 0;
            while (enumerator.MoveNext())
            {
                Add(enumerator.Current);
                if (++loaded == eachTimeCount) return;
            }
            finished = true;
        }

        public void Refresh()
        {
            enumerator = underlying.GetEnumerator();
            LoadMore();
        }

        public LazyLoadCollection(IEnumerable<T> underlying, int eachTimeCount = 10)
        {
            this.underlying = underlying;
            this.eachTimeCount = eachTimeCount;
            if (underlying is INotifyCollectionChanged notifiable)
            {
                notifiable.CollectionChanged += Notifiable_CollectionChanged;
            }
            Refresh();
        }

        private void Notifiable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Clear();
            enumerator = underlying.GetEnumerator();
            finished = false;
            LoadMore(); 
        }

        public void Dispose()
        {
            if (underlying is IDisposable disposable) disposable.Dispose();
        }
    }
}
