using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Meta;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Models.Collections
{
    internal class DescendantsCollection : IEnumerableExpression<IModelElement>
    {
        public IModelElement Element { get; private set; }
        private Notifiable notifiable;

        public DescendantsCollection(IModelElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            this.Element = element;
        }

        public INotifyEnumerable<IModelElement> AsNotifiable()
        {
            if (notifiable == null) notifiable = new Notifiable(Element);
            return notifiable;
        }

        public IEnumerator<IModelElement> GetEnumerator()
        {
            return Element.Children.SelectRecursive(e => e.Children).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        private class Notifiable : INotifyEnumerable<IModelElement>
        {
            private readonly IModelElement element;
            private readonly ISuccessorList successors;
            private readonly ExecutionMetaData metadata;

            private CollectionChangedNotificationResult<IModelElement> notification;
            private bool isNotified;

            public Notifiable(IModelElement element)
            {
                this.element = element;
                this.successors = new MultiSuccessorList();
                this.metadata = new ExecutionMetaData();
                this.notification = CollectionChangedNotificationResult<IModelElement>.Create(this, false);

                element.BubbledChange += Element_BubbledChange;
            }

            private void Element_BubbledChange(object sender, BubbledChangeEventArgs e)
            {
                if (e.ChangeType == ChangeType.ElementCreated)
                {
                    notification.AddedItems.Add(e.Element);
                    notification.AddedItems.AddRange(e.Element.Descendants());
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.Element));
                }
                else if (e.ChangeType == ChangeType.ElementDeleted)
                {
                    notification.RemovedItems.Add(e.Element);
                    notification.RemovedItems.AddRange(e.Element.Descendants());
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.Element));
                }
                else
                {
                    return;
                }
                if (!isNotified)
                {
                    isNotified = true;
                    ExecutionEngine.Current.InvalidateNode(this);
                }
            }

            public ISuccessorList Successors => successors;

            public IEnumerable<INotifiable> Dependencies => Enumerable.Empty<INotifiable>();

            public ExecutionMetaData ExecutionMetaData => metadata;

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void Dispose()
            {
            }

            public IEnumerator<IModelElement> GetEnumerator()
            {
                return element.Children.SelectRecursive(e => e.Children).GetEnumerator();
            }

            public INotificationResult Notify(IList<INotificationResult> sources)
            {
                isNotified = false;
                var result = notification;
                notification = CollectionChangedNotificationResult<IModelElement>.Create(this, false);
                return result;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
