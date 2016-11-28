using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class ObservableOppositeOrderedSet<TParent, TCollected> : ObservableOrderedSet<TCollected>
    {
        public TParent Parent { get; private set; }

        protected abstract void SetOpposite(TCollected item, TParent newParent);

        protected ObservableOppositeOrderedSet(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override void Clear()
        {
            var elements = this.ToArray();
            base.Clear();
            foreach (var item in elements)
            {
                SetOpposite(item, default(TParent));
            }
        }

        protected override void OnInsertItem(TCollected item)
        {
            SetOpposite(item, Parent);
            base.OnInsertItem(item);
        }

        protected override void OnRemoveItem(TCollected item)
        {
            SetOpposite(item, default(TParent));
            base.OnRemoveItem(item);
        }

        protected override void OnReplaceItem(TCollected oldItem, TCollected newItem)
        {
            SetOpposite(oldItem, default(TParent));
            SetOpposite(newItem, Parent);
            base.OnReplaceItem(oldItem, newItem);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class ObservableOppositeSet<TParent, TCollected> : ObservableSet<TCollected>
    {
        public TParent Parent { get; private set; }

        protected abstract void SetOpposite(TCollected item, TParent newParent);

        protected ObservableOppositeSet(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override void Clear()
        {
            var elements = this.ToArray();
            base.Clear();
            foreach (var item in elements)
            {
                SetOpposite(item, default(TParent));
            }
        }

        protected override void OnInsertItem(TCollected item)
        {
            base.OnInsertItem(item);
            SetOpposite(item, Parent);
        }

        protected override void OnRemoveItem(TCollected item)
        {
            base.OnRemoveItem(item);
            SetOpposite(item, default(TParent));
        }

        protected override void OnReplaceItem(TCollected oldItem, TCollected newItem)
        {
            base.OnReplaceItem(oldItem, newItem);
            SetOpposite(oldItem, default(TParent));
            SetOpposite(newItem, Parent);
        }

    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class ObservableOppositeList<TParent, TCollected> : ObservableList<TCollected>
    {
        public TParent Parent { get; private set; }

        protected abstract void SetOpposite(TCollected item, TParent newParent);

        private bool noModification;

        protected ObservableOppositeList(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void ClearItems()
        {
            if (!noModification)
            {
                base.ClearItems();
            }
        }

        protected override void BeforeClearPropagates(TCollected[] elements)
        {
            noModification = true;
            foreach (var item in elements)
            {
                SetOpposite(item, default(TParent));
            }
            noModification = false;
        }

        protected override void InsertItem(int index, TCollected item)
        {
            if (!noModification)
            {
                base.InsertItem(index, item);
            }
        }

        protected override void BeforeInsertPropagates(int index, TCollected item)
        {
            noModification = true;
            SetOpposite(item, Parent);
            noModification = false;
        }

        protected override void RemoveItem(int index)
        {
            if (!noModification)
            {
                base.RemoveItem(index);
            }
        }

        protected override void BeforeRemovePropagates(int index, TCollected item)
        {
            noModification = true;
            SetOpposite(item, default(TParent));
            noModification = false;
        }

        protected override void SetItem(int index, TCollected item)
        {
            if (!noModification)
            {
                base.SetItem(index, item);
            }
        }

        protected override void BeforeSetItemPropagates(int index, TCollected item, TCollected oldItem)
        {
            noModification = true;
            SetOpposite(this[index], default(TParent));
            SetOpposite(item, Parent);
            noModification = false;
        }
    }
}
