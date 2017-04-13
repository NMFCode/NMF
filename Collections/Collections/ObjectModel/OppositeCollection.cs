using NMF.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class OppositeOrderedSet<TParent, TCollected> : OrderedSet<TCollected>
    {
        public TParent Parent { get; private set; }

        protected abstract void SetOpposite(TCollected item, TParent newParent);

        protected OppositeOrderedSet(TParent parent)
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

        protected override void OnInsertItem(TCollected item, int index)
        {
            SetOpposite(item, Parent);
            base.OnInsertItem(item, index);
        }

        protected override void OnRemoveItem(TCollected item, int index)
        {
            SetOpposite(item, default(TParent));
            base.OnRemoveItem(item, index);
        }

        protected override void OnReplaceItem(TCollected oldItem, TCollected newItem, int index)
        {
            SetOpposite(oldItem, default(TParent));
            SetOpposite(newItem, Parent);
            base.OnReplaceItem(oldItem, newItem, index);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class OppositeSet<TParent, TCollected> : DecoratedSet<TCollected>
    {
        public TParent Parent { get; private set; }

        protected abstract void SetOpposite(TCollected item, TParent newParent);

        protected OppositeSet(TParent parent)
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

        public override bool Add(TCollected item)
        {
            if (base.Add(item))
            {
                SetOpposite(item, Parent);
                return true;
            }
            return false;
        }

        public override bool Remove(TCollected item)
        {
            if (base.Remove(item))
            {
                SetOpposite(item, default(TParent));
                return true;
            }
            return false;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class OppositeList<TParent, TCollected> : Collection<TCollected>
    {
        public TParent Parent { get; private set; }

        protected abstract void SetOpposite(TCollected item, TParent newParent);

        protected OppositeList(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void ClearItems()
        {
            var elements = this.ToArray();
            base.ClearItems();
            foreach (var item in elements)
            {
                SetOpposite(item, default(TParent));
            }
        }

        protected override void InsertItem(int index, TCollected item)
        {
            base.InsertItem(index, item);
            SetOpposite(item, Parent);
        }

        protected override void RemoveItem(int index)
        {
            SetOpposite(this[index], default(TParent));
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, TCollected item)
        {
            var old = this[index];
            base.SetItem(index, item);
            if (!object.Equals(old, item))
            {
                SetOpposite(this[index], default(TParent));
                SetOpposite(item, Parent);
            }
        }
    }
}
