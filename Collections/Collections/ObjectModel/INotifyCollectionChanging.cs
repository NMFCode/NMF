using System;
using System.Collections.Specialized;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes an interface for collections that notify when a change attempt is performed
    /// </summary>
    public interface INotifyCollectionChanging
    {
        /// <summary>
        /// Gets raised when an attempt is made to change a given collection
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;
    }
}
