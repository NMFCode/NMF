using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    public abstract class ReferenceCollection : ICollectionExpression<IModelElement>, INotifyCollection<IModelElement>
    {
        private int attachedCount = 0;

        public abstract IEnumerator<IModelElement> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Attach()
        {
            attachedCount++;
            if (attachedCount == 1)
            {
                AttachCore();
            }
        }

        public void Detach()
        {
            attachedCount--;
            if (attachedCount == 0)
            {
                DetachCore();
            }
        }

        protected abstract void AttachCore();
        protected abstract void DetachCore();

        public bool IsAttached
        {
            get { return attachedCount > 0; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public abstract void Add(IModelElement item);

        public abstract void Clear();

        public abstract bool Contains(IModelElement item);

        public abstract void CopyTo(IModelElement[] array, int arrayIndex);

        public abstract int Count
        {
            get;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public abstract bool Remove(IModelElement item);

        public INotifyCollection<IModelElement> AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable<IModelElement> IEnumerableExpression<IModelElement>.AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return this;
        }

        protected void PropagateCollectionChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        protected void PropagateValueChanges(object sender, ValueChangedEventArgs e)
        {
            if (e.OldValue == null)
            {
                if (e.NewValue != null)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewValue));
                }
            }
            else
            {
                if (e.NewValue == null)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldValue));
                }
                else
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewValue, e.OldValue));
                }
            }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
