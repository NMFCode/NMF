using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    public interface INotifyCollectionChanging
    {
        event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;
    }
}
