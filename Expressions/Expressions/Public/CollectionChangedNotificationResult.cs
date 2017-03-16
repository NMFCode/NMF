using System;
using System.Collections.Generic;

namespace NMF.Expressions.Public
{
    public class CollectionChangedNotificationResult<T> : INotificationResult
    {
        public INotifiable Source { get; }
        public ICollection<T> OldCollection { get; }
        public ICollection<T> NewCollection { get; }
        public bool Changed => true;

        public CollectionChangedNotificationResult(INotifiable source, ICollection<T> oldCollection, ICollection<T> newCollection)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Source = source;
            OldCollection = oldCollection;
            NewCollection = newCollection;
        }
    }
}
