using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    public class DescendantsCollection : IEnumerableExpression<IModelElement>, INotifyEnumerable<IModelElement>
    {
        public IModelElement Element { get; private set; }

        private INotifyEnumerable<IModelElement> childrenCollection;
        private Dictionary<IModelElement, INotifyEnumerable<IModelElement>> childCollections;
        private int attachedCount = 0;

        public DescendantsCollection(IModelElement element)
        {
            this.Element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public INotifyEnumerable<IModelElement> AsNotifiable()
        {
            Attach();
            return this;
        }

        public IEnumerator<IModelElement> GetEnumerator()
        {
            var stack = new Stack<IModelElement>(Element.Children);
            while (stack.Count > 0)
            {
                var element = stack.Pop();
                if (element == null) continue;
                yield return element;
                foreach (var ancestor in element.Children)
                {
                    stack.Push(ancestor);
                }
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


        public void Attach()
        {
            attachedCount++;
            if (attachedCount == 1)
            {
                AttachCore();
            }
        }

        protected void AttachCore()
        {
            if (childrenCollection == null)
            {
                childrenCollection = Element.Children.AsNotifiable();
            }
            else
            {
                childrenCollection.Attach();
            }
            childrenCollection.CollectionChanged += ChildrenChanged;
            if (childCollections == null)
            {
                childCollections = new Dictionary<IModelElement, INotifyEnumerable<IModelElement>>();
            }
            foreach (var child in childrenCollection)
            {
                var descendants = child.Descendants().AsNotifiable();
                descendants.CollectionChanged += PropagateChildChanges;
                childCollections.Add(child, descendants);
            }
        }

        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                OnCollectionChanged(e);
            }
            else
            {
                if (e.NewItems != null)
                {
                    var added = new List<IModelElement>();
                    foreach (IModelElement child in e.NewItems)
                    {
                        added.Add(child);
                        var descendants = child.Descendants().AsNotifiable();
                        added.AddRange(descendants);
                        childCollections.Add(child, descendants);
                    }
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added));
                }
                if (e.OldItems != null)
                {
                    var removed = new List<IModelElement>();
                    foreach (IModelElement child in e.OldItems)
                    {
                        INotifyEnumerable<IModelElement> descendants;
                        if (child != null && childCollections.TryGetValue(child, out descendants))
                        {
                            removed.Add(child);
                            removed.AddRange(descendants);
                            childCollections.Remove(child);
                        }
                    }
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
                }
            }
        }

        private void PropagateChildChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        public void Detach()
        {
            attachedCount--;
            if (attachedCount == 0)
            {
                DetachCore();
            }
        }

        protected void DetachCore()
        {
            foreach (var child in childCollections.Values)
            {
                child.CollectionChanged -= PropagateChildChanges;
            }
            childCollections.Clear();
            childrenCollection.CollectionChanged -= ChildrenChanged;
        }

        public bool IsAttached
        {
            get { return attachedCount > 0; }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
