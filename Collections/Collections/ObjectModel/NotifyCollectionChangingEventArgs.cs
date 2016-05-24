using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    public class NotifyCollectionChangingEventArgs : EventArgs
    {
        public NotifyCollectionChangedAction Action { get; private set; }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action)
        {
            Action = action;
        }
    }
}
