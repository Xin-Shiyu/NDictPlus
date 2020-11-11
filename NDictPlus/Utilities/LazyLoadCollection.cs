using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace NDictPlus.Utilities
{
    public class LazyLoadCollection<T> : ObservableCollection<T>
    {
        readonly IEnumerable<T> underlying;
        readonly int eachTimeCount;

        IEnumerator<T> enumerator;
        bool finished = false;

        public void LoadMore()
        {
            if (finished) return;
            for (int i = 0; i < eachTimeCount; ++i)
            {
                Add(enumerator.Current);
                if (!enumerator.MoveNext())
                {
                    finished = true;
                    break;
                }
            }
        }

        public void Refresh()
        {
            enumerator = underlying.GetEnumerator();
            enumerator.MoveNext();
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
            enumerator.MoveNext();
            LoadMore();
        }
    }
}
