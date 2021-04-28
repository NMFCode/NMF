using NMF.Expressions;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    internal class AncestorCollection : IEnumerableExpression<IModelElement>
    {
        public IModelElement Element { get; private set; }
        private Notifiable notifiable;

        public AncestorCollection(IModelElement element)
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
            var element = Element.Parent;
            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
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

                element.ParentChanged += Ancestor_ParentChanged;
                foreach (var ancestor in this)
                {
                    ancestor.ParentChanged += Ancestor_ParentChanged;
                }
            }

            private void Ancestor_ParentChanged(object sender, ValueChangedEventArgs e)
            {
                var newParent = e.NewValue as IModelElement;
                var oldParent = e.OldValue as IModelElement;

                while (newParent != null)
                {
                    if (!notification.RemovedItems.Remove(newParent))
                    {
                        notification.AddedItems.Add(newParent);
                    }
                    newParent.ParentChanged += Ancestor_ParentChanged;
                    newParent = newParent.Parent;
                }
                while (oldParent != null)
                {
                    if (!notification.AddedItems.Remove(oldParent))
                    {
                        notification.RemovedItems.Add(oldParent);
                    }
                    oldParent.ParentChanged -= Ancestor_ParentChanged;
                    oldParent = oldParent.Parent;
                }

                if (notification.AddedItems.Count > 0)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, notification.AddedItems));
                }
                if (notification.RemovedItems.Count > 0)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, notification.RemovedItems));
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
                var element = this.element.Parent;
                while (element != null)
                {
                    yield return element;
                    element = element.Parent;
                }
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
