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
    public class DescendantsCollection : IEnumerableExpression<IModelElement>, INotifyEnumerable<IModelElement>
    {
        private readonly SuccessorList successors = new SuccessorList();
        public IModelElement Element { get; private set; }

        private INotifyEnumerable<IModelElement> childrenCollection;
        private Dictionary<IModelElement, INotifyEnumerable<IModelElement>> childCollections;

        public DescendantsCollection(IModelElement element)
        {
            this.Element = element;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

        public INotifyEnumerable<IModelElement> AsNotifiable()
        {
            return this;
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


        public void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Add(this);
            OnAttach();
        }

        protected void OnAttach()
        {
            if (childrenCollection == null)
            {
                childrenCollection = Element.Children.AsNotifiable();
            }
            childrenCollection.Successors.Add(this);

            if (childCollections == null)
            {
                childCollections = new Dictionary<IModelElement, INotifyEnumerable<IModelElement>>();
            }
            foreach (var child in childrenCollection)
            {
                var descendants = child.Descendants().AsNotifiable();
                descendants.Successors.Add(this);
                childCollections.Add(child, descendants);
            }
        }

        public void Detach()
        {
            OnDetach();
            foreach (var dep in Dependencies)
                dep.Successors.Remove(this);
        }

        protected void OnDetach()
        {
            foreach (var child in childCollections.Values)
            {
                child.Successors.Remove(this);
            }
            childCollections.Clear();
            childrenCollection.Successors.Remove(this);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IList<INotifiable> Successors { get { return successors; } }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (childrenCollection != null)
                    yield return childrenCollection;
                if (childCollections != null)
                    foreach (var child in childCollections.Values)
                        yield return child;
            }
        }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

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
            Successors.Clear();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<IModelElement>();
            var removed = new List<IModelElement>();

            foreach (CollectionChangedNotificationResult<IModelElement> change in sources)
            {
                if (change.IsReset)
                    return new CollectionChangedNotificationResult<IModelElement>(this);

                if (change.Source == childrenCollection)
                {
                    foreach (var child in change.AllRemovedItems)
                    {
                        INotifyEnumerable<IModelElement> descendants;
                        if (child != null && childCollections.TryGetValue(child, out descendants))
                        {
                            removed.Add(child);
                            removed.AddRange(descendants);
                            descendants.Successors.Remove(this);
                            childCollections.Remove(child);
                        }
                    }
                    
                    foreach (var child in change.AllAddedItems)
                    {
                        added.Add(child);
                        var descendants = child.Descendants().AsNotifiable();
                        descendants.Successors.Add(this);
                        added.AddRange(descendants);
                        childCollections.Add(child, descendants);
                    }
                }
                else
                {
                    removed.AddRange(change.AllRemovedItems);
                    added.AddRange(change.AllAddedItems);
                }
            }

            if (added.Count == 0 && removed.Count == 0)
                return new UnchangedNotificationResult(this);

            if (removed.Count > 0)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
            if (added.Count > 0)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added));

            return new CollectionChangedNotificationResult<IModelElement>(this, added, removed);
        }
    }
}
