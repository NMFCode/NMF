using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Dynamic
{
    internal class DecomposedCollection : ICollectionExpression<IModelElement>
    {
        private readonly ICollection<IReferenceProperty> _referenceProperties;

        public DecomposedCollection(ICollection<IReferenceProperty> referenceProperties)
        {
            _referenceProperties = referenceProperties;
        }

        INotifyEnumerable<IModelElement> IEnumerableExpression<IModelElement>.AsNotifiable()
        {
            return AsNotifiable();
        }

        private IEnumerable<IModelElement> ElementsCore
        {
            get
            {
                foreach (var reference in _referenceProperties)
                {
                    if (reference.Collection != null)
                    {
                        foreach (var element in reference.Collection.OfType<IModelElement>())
                        {
                            yield return element;
                        }
                    }
                    else
                    {
                        var value = reference.GetValue(0);
                        if (value is IModelElement modelElement)
                        {
                            yield return modelElement;
                        }
                    }
                }
            }
        }

        public int Count => _referenceProperties.Sum(p => p.Count);

        public bool IsReadOnly => false;

        public IEnumerator<IModelElement> GetEnumerator()
        {
            return ElementsCore.GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public INotifyCollection<IModelElement> AsNotifiable()
        {
            return new Notifiable(this);
        }

        public void Add(IModelElement item)
        {
            foreach (var prop in _referenceProperties)
            {
                if (prop.TryAdd(item))
                {
                    return;
                }
            }
            throw new InvalidOperationException("Item could not be added.");
        }

        public void Clear()
        {
            foreach(var prop in _referenceProperties)
            {
                prop.Reset();
            }
        }

        public bool Contains(IModelElement item)
        {
            foreach (var prop in _referenceProperties)
            {
                if (prop.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(IModelElement[] array, int arrayIndex)
        {
            foreach (var item in ElementsCore)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(IModelElement item)
        {
            foreach (var prop in _referenceProperties)
            {
                if (prop.TryRemove(item))
                {
                    return true;
                }
            }
            return false;
        }

        private class Notifiable : ObservableEnumerable<IModelElement>, INotifyCollection<IModelElement>
        {
            private readonly DecomposedCollection _parent;

            public Notifiable(DecomposedCollection parent)
            {
                _parent = parent;
            }

            public override IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    foreach (var prop in _parent._referenceProperties)
                    {
                        if (prop.Collection is INotifyEnumerable notifyEnumerable)
                        {
                            yield return notifyEnumerable;
                        }
                        else
                        {
                            yield return prop.ReferencedElement;
                        }
                    }
                }
            }

            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return _parent.GetEnumerator();
            }

            public override int Count => _parent.Count;

            public override void Add(IModelElement item) => _parent.Add(item);

            public override void Clear() => _parent.Clear();

            public override bool Contains(IModelElement item) => _parent.Contains(item);

            public override bool IsReadOnly => false;

            public override bool Remove(IModelElement item) => _parent.Remove(item);

            public override INotificationResult Notify(IList<INotificationResult> sources)
            {
                var result = CollectionChangedNotificationResult<IModelElement>.Create(this, false);

                foreach (var source in sources)
                {
                    if (source is ICollectionChangedNotificationResult collectionChange)
                    {
                        if (collectionChange.IsReset)
                        {
                            result.TurnIntoReset();
                            break;
                        }
                        if (collectionChange.AddedItems != null)
                        {
                            result.AddedItems.AddRange(collectionChange.AddedItems.OfType<IModelElement>());
                        }
                        if (collectionChange.RemovedItems != null)
                        {
                            result.RemovedItems.AddRange(collectionChange.RemovedItems.OfType<IModelElement>());
                        }
                        result.OldItemsStartIndex = -1;
                        result.NewItemsStartIndex = -1;
                    }
                    else if (source is IValueChangedNotificationResult valueChange)
                    {
                        if (valueChange.OldValue is IModelElement oldValue)
                        {
                            result.RemovedItems.Add(oldValue);
                        }
                        if (valueChange.NewValue is IModelElement newValue)
                        {
                            result.AddedItems.Add(newValue);
                        }
                    }
                }

                return result;
            }
        }
    }
}
