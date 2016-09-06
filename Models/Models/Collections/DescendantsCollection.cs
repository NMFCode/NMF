using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    public class DescendantsCollection : IEnumerableExpression<IModelElement>, INotifyCollectionChanged
    {
        public IModelElement Element { get; private set; }

        private INotifyEnumerable<IModelElement> childrenCollection;
        private Dictionary<IModelElement, IEnumerableExpression<IModelElement>> childCollections;

        public DescendantsCollection(IModelElement element)
        {
            this.Element = element;
            Attach();
        }

        public INotifyEnumerable<IModelElement> AsNotifiable()
        {
            return this.WithUpdates();
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

        private void Attach()
        {
            if (childrenCollection == null)
            {
                childrenCollection = Element.Children.AsNotifiable();
                childrenCollection.Successors.Add(null);
                childrenCollection.CollectionChanged += ChildrenChanged;
            }

            if (childCollections == null)
            {
                childCollections = new Dictionary<IModelElement, IEnumerableExpression<IModelElement>>();
            }
            foreach (var child in childrenCollection)
            {
                var descendants = child.Descendants();
                ((INotifyCollectionChanged)descendants).CollectionChanged += ChildDescendantsChanged;
                childCollections.Add(child, descendants);
            }
        }

        private void Detach()
        {
            foreach (INotifyCollectionChanged child in childCollections.Values)
            {
                child.CollectionChanged -= ChildDescendantsChanged;
            }
            childCollections.Clear();

            childrenCollection.CollectionChanged -= ChildrenChanged;
            childrenCollection.Successors.Remove(null);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Detach();
        }

        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Detach();
                Attach();
                OnCollectionChanged(e);
                return;
            }
            
            if (e.OldItems != null)
            {
                var removed = new List<IModelElement>();
                foreach (IModelElement child in e.OldItems)
                {
                    IEnumerableExpression<IModelElement> descendants;
                    if (child != null && childCollections.TryGetValue(child, out descendants))
                    {
                        removed.Add(child);
                        removed.AddRange(descendants);
                        ((INotifyCollectionChanged)descendants).CollectionChanged -= ChildDescendantsChanged;
                        childCollections.Remove(child);
                    }
                }

                if (removed.Count > 0)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
            }

            if (e.NewItems != null)
            {
                var added = new List<IModelElement>();
                foreach (IModelElement child in e.NewItems)
                {
                    added.Add(child);
                    var descendants = child.Descendants();
                    ((INotifyCollectionChanged)descendants).CollectionChanged += ChildDescendantsChanged;
                    added.AddRange(descendants);
                    childCollections.Add(child, descendants);
                }
            }
        }

        private void ChildDescendantsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }
    }
}
