using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    public interface INotifyCollectionChanging
    {
        event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;
    }
}
