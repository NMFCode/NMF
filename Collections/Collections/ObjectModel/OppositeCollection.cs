using NMF.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes an abstract base class for an ordered set with opposites
    /// </summary>
    /// <typeparam name="TParent">the type of the parent element</typeparam>
    /// <typeparam name="TCollected">the collection item type</typeparam>
    public abstract class OppositeOrderedSet<TParent, TCollected> : OrderedSet<TCollected>
    {
        /// <summary>
        /// Gets the parent element
        /// </summary>
        public TParent Parent { get; private set; }

        /// <summary>
        /// Sets the opposite of the given item
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="newParent">the new parent or null, if the item is removed from the collection</param>
        protected abstract void SetOpposite(TCollected item, TParent newParent);

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent model element</param>
        /// <exception cref="ArgumentNullException">thrown if parent is null</exception>
        protected OppositeOrderedSet(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            var elements = this.ToArray();
            base.Clear();
            foreach (var item in elements)
            {
                SetOpposite(item, default(TParent));
            }
        }

        /// <inheritdoc />
        public override bool Add(TCollected item)
        {
            if (base.Add(item))
            {
                SetOpposite(item, Parent);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override bool Remove(TCollected item, int index)
        {
            if (base.Remove(item, index))
            {
                SetOpposite(item, default);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override void Replace(int index, TCollected oldValue, TCollected newValue)
        {
            base.Replace(index, oldValue, newValue);
            SetOpposite(oldValue, default);
            SetOpposite(newValue, Parent);
        }
    }

    /// <summary>
    /// Denotes an abstract base class for a set with opposites
    /// </summary>
    /// <typeparam name="TParent">the type of the parent</typeparam>
    /// <typeparam name="TCollected">the type of the collection items</typeparam>
    public abstract class OppositeSet<TParent, TCollected> : DecoratedSet<TCollected>
    {
        /// <summary>
        /// Gets the parent element
        /// </summary>
        public TParent Parent { get; private set; }

        /// <summary>
        /// Sets the opposite of the given item
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="newParent">the new parent or null, if the item is removed from the collection</param>
        protected abstract void SetOpposite(TCollected item, TParent newParent);

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent model element</param>
        /// <exception cref="ArgumentNullException">thrown if parent is null</exception>
        protected OppositeSet(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            var elements = this.ToArray();
            base.Clear();
            foreach (var item in elements)
            {
                SetOpposite(item, default);
            }
        }

        /// <inheritdoc />
        public override bool Add(TCollected item)
        {
            if (base.Add(item))
            {
                SetOpposite(item, Parent);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override bool Remove(TCollected item)
        {
            if (base.Remove(item))
            {
                SetOpposite(item, default);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Denotes the abstract base class for a list with opposites
    /// </summary>
    /// <typeparam name="TParent">the type of the parent element</typeparam>
    /// <typeparam name="TCollected">the type of collection items</typeparam>
    public abstract class OppositeList<TParent, TCollected> : Collection<TCollected>
    {
        /// <summary>
        /// Gets the parent element
        /// </summary>
        public TParent Parent { get; private set; }
        
        /// <summary>
        /// Sets the opposite of the given item
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="newParent">the new parent or null, if the item is removed from the collection</param>
        protected abstract void SetOpposite(TCollected item, TParent newParent);

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent model element</param>
        /// <exception cref="ArgumentNullException">thrown if parent is null</exception>
        protected OppositeList(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
        protected override void ClearItems()
        {
            var elements = this.ToArray();
            base.ClearItems();
            foreach (var item in elements)
            {
                SetOpposite(item, default);
            }
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, TCollected item)
        {
            base.InsertItem(index, item);
            SetOpposite(item, Parent);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            SetOpposite(this[index], default);
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, TCollected item)
        {
            var old = this[index];
            base.SetItem(index, item);
            if (!object.Equals(old, item))
            {
                SetOpposite(this[index], default);
                SetOpposite(item, Parent);
            }
        }
    }
}
