using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    internal class AncestorTreeCollection : IEnumerableExpression<ModelTreeItem>
    {
        public IModelElement Element { get; private set; }
        private Notifiable notifiable;

        public AncestorTreeCollection(IModelElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            this.Element = element;
        }

        public INotifyEnumerable<ModelTreeItem> AsNotifiable()
        {
            if (notifiable == null) notifiable = new Notifiable(Element);
            return notifiable;
        }

        public IEnumerator<ModelTreeItem> GetEnumerator()
        {
            var element = Element;
            while (element.Parent != null)
            {
                yield return new ModelTreeItem(element.Parent, element);
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

        private class Notifiable : INotifyEnumerable<ModelTreeItem>
        {
            private readonly IModelElement element;
            private readonly ISuccessorList successors;
            private readonly ExecutionMetaData metadata;

            private CollectionChangedNotificationResult<ModelTreeItem> notification;
            private bool isNotified;

            public Notifiable(IModelElement element)
            {
                this.element = element;
                this.successors = new MultiSuccessorList();
                this.metadata = new ExecutionMetaData();
                this.notification = CollectionChangedNotificationResult<ModelTreeItem>.Create(this, false);

                element.ParentChanged += Ancestor_ParentChanged;
                foreach (var ancestor in element.Ancestors())
                {
                    ancestor.ParentChanged += Ancestor_ParentChanged;
                }
            }

            private void Ancestor_ParentChanged(object sender, ValueChangedEventArgs e)
            {
                var el = sender as IModelElement;
                var newParent = e.NewValue as IModelElement;
                var oldParent = e.OldValue as IModelElement;

                var current = el;
                while (newParent != null)
                {
                    if (!notification.RemovedItems.Remove(new ModelTreeItem(newParent, current)))
                    {
                        notification.AddedItems.Add(new ModelTreeItem(newParent, current));
                    }
                    newParent.ParentChanged += Ancestor_ParentChanged;
                    current = newParent;
                    newParent = newParent.Parent;
                }
                current = el;
                while (oldParent != null)
                {
                    if (!notification.AddedItems.Remove(new ModelTreeItem(oldParent, current)))
                    {
                        notification.RemovedItems.Add(new ModelTreeItem(oldParent, current));
                    }
                    oldParent.ParentChanged -= Ancestor_ParentChanged;
                    current = oldParent;
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

            public IEnumerator<ModelTreeItem> GetEnumerator()
            {
                var element = this.element;
                while (element.Parent != null)
                {
                    yield return new ModelTreeItem(element.Parent, element);
                    element = element.Parent;
                }
            }

            public INotificationResult Notify(IList<INotificationResult> sources)
            {
                isNotified = false;
                var result = notification;
                notification = CollectionChangedNotificationResult<ModelTreeItem>.Create(this, false);
                return result;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
