using System;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes an abstract base class for an observable ordered set with opposites
    /// </summary>
    /// <typeparam name="TParent">The type of the parent element</typeparam>
    /// <typeparam name="TCollected">The type of the collected elements</typeparam>
    public abstract class ObservableOppositeOrderedSet<TParent, TCollected> : ObservableOrderedSet<TCollected>
    {
        /// <summary>
        /// Gets the parent element
        /// </summary>
        public TParent Parent { get; private set; }

        /// <summary>
        /// Sets the opposite value
        /// </summary>
        /// <param name="item">The item to set the opposite</param>
        /// <param name="newParent">The new parent or null, if the element was removed</param>
        protected abstract void SetOpposite(TCollected item, TParent newParent);

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent element</param>
        /// <exception cref="ArgumentNullException">Thrown if the parent element is null</exception>
        protected ObservableOppositeOrderedSet(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            var elements = this.ToArray();
            if (!RequireEvents())
            {
                SilentClear();
                UnsetOpposites(elements);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                UnsetOpposites(elements);
                OnCollectionChanged(e);
            }
        }

        private void UnsetOpposites(TCollected[] elements)
        {
            var elementIndex = 0;
            try
            {
                for (elementIndex = 0; elementIndex < elements.Length; elementIndex++)
                {
                    SetOpposite(elements[elementIndex], default(TParent));
                }
            }
            catch
            {
                for (int i = elementIndex; i < elements.Length; i++)
                {
                    SilentAdd(elements[elementIndex]);
                }
            }
        }

        /// <inheritdoc />
        public override bool Add(TCollected item)
        {
            if (!RequireEvents())
            {
                if (SilentAdd(item))
                {
                    TrySetOpposite(item);
                    return true;
                }
                return false;
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count);
                OnCollectionChanging(e);
                if (SilentAdd(item))
                {
                    TrySetOpposite(item);
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        private void TrySetOpposite(TCollected item)
        {
            try
            {
                SetOpposite(item, Parent);
            }
            catch
            {
                SilentRemove(item, Count - 1);
                throw;
            }
        }

        /// <inheritdoc />
        public override void Insert(int index, TCollected item)
        {
            if (!RequireEvents())
            {
                SilentInsert(index, item);
                TrySetOpposite(item);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                OnCollectionChanging(e);
                SilentInsert(index, item);
                TrySetOpposite(item);
                OnCollectionChanged(e);
            }
        }

        /// <inheritdoc />
        protected override bool Remove(TCollected item, int index)
        {
            if (!RequireEvents())
            {
                if(SilentRemove(item, index))
                {
                    TryUnsetOpposite(item, index);
                    return true;
                }
                return false;
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                OnCollectionChanging(e);
                if (SilentRemove(item, index))
                {
                    TryUnsetOpposite(item, index);
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        private void TryUnsetOpposite(TCollected item, int index)
        {
            try
            {
                SetOpposite(item, default(TParent));
            }
            catch
            {
                SilentInsert(index, item);
                throw;
            }
        }

        /// <inheritdoc />
        protected override void Replace(int index, TCollected oldValue, TCollected newValue)
        {
            if (!RequireEvents())
            {
                SilentReplace(index, oldValue, newValue);
                try
                {
                    SetOpposite(oldValue, default);
                    SetOpposite(newValue, Parent);
                }
                catch
                {
#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
                    SilentReplace(index, newValue, oldValue);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters
                    throw;
                }
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                OnCollectionChanging(e);
                SilentReplace(index, oldValue, newValue);
                try
                {
                    SetOpposite(oldValue, default);
                    SetOpposite(newValue, Parent);
                }
                catch
                {
#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
                    SilentReplace(index, newValue, oldValue);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters
                    throw;
                }
                OnCollectionChanged(e);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Opposite OrderedSet Count={Count}]";
        }
    }

    /// <summary>
    /// Denotes the abstract base class for an observable set with opposites
    /// </summary>
    /// <typeparam name="TParent">The type of the parent element</typeparam>
    /// <typeparam name="TCollected">The type of the elements</typeparam>
    public abstract class ObservableOppositeSet<TParent, TCollected> : ObservableSet<TCollected>
    {
        /// <summary>
        /// Gets the parent for this collection
        /// </summary>
        public TParent Parent { get; private set; }

        /// <summary>
        /// Sets the opposite
        /// </summary>
        /// <param name="item">the item for which the opposite should be set</param>
        /// <param name="newParent">the new parent or null, if the element is deleted</param>
        protected abstract void SetOpposite(TCollected item, TParent newParent);

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent element</param>
        /// <exception cref="ArgumentNullException">Thrown if the parent is null</exception>
        protected ObservableOppositeSet(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            var elements = this.ToArray();
            if (!RequireEvents())
            {
                SilentClear();
                UnsetOpposites(elements);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                UnsetOpposites(elements);
                OnCollectionChanged(e);
            }
        }

        private void UnsetOpposites(TCollected[] elements)
        {
            var elementIndex = 0;
            try
            {
                for (elementIndex = 0; elementIndex < elements.Length; elementIndex++)
                {
                    SetOpposite(elements[elementIndex], default(TParent));
                }
            }
            catch
            {
                for (int i = elementIndex; i < elements.Length; i++)
                {
                    SilentAdd(elements[elementIndex]);
                }
            }
        }

        /// <inheritdoc />
        public override bool Add(TCollected item)
        {
            if (!RequireEvents())
            {
                if (SilentAdd(item))
                {
                    SetOpposite(item, Parent);
                    return true;
                }
                return false;
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
                OnCollectionChanging(e);
                if (SilentAdd(item))
                {
                    SetOpposite(item, Parent);
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        /// <inheritdoc />
        public override bool Remove(TCollected item)
        {
            if (!RequireEvents())
            {
                if (SilentRemove(item))
                {
                    SetOpposite(item, default);
                    return true;
                }
                return false;
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item);
                OnCollectionChanging(e);
                if (SilentRemove(item))
                {
                    SetOpposite(item, default);
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Opposite Set Count={Count}]";
        }
    }

    /// <summary>
    /// Denotes the abstract base class for an observable opposite list
    /// </summary>
    /// <typeparam name="TParent">the parent element</typeparam>
    /// <typeparam name="TCollected">the type of collected elements</typeparam>
    public abstract class ObservableOppositeList<TParent, TCollected> : ObservableList<TCollected>
    {
        /// <summary>
        /// Gets the parent element
        /// </summary>
        public TParent Parent { get; private set; }

        /// <summary>
        /// Sets the opposite element
        /// </summary>
        /// <param name="item">The item for which the opposite should be set</param>
        /// <param name="newParent">The new parent or null, if the item is removed</param>
        protected abstract void SetOpposite(TCollected item, TParent newParent);

        private bool noModification;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent element</param>
        /// <exception cref="ArgumentNullException">thrown if the parent element is null</exception>
        protected ObservableOppositeList(TParent parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        /// <inheritdoc />
        protected override void ClearItems()
        {
            if (!noModification)
            {
                var elements = this.ToArray();
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    OnCollectionChanging(e);
                    Items.Clear();
                    UnsetOpposites(elements);
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.Clear();
                    UnsetOpposites(elements);
                }
            }
        }


        private void UnsetOpposites(TCollected[] elements)
        {
            var elementIndex = 0;
            try
            {
                noModification = true;
                for (elementIndex = 0; elementIndex < elements.Length; elementIndex++)
                {
                    SetOpposite(elements[elementIndex], default);
                }
            }
            catch
            {
                for (int i = elementIndex; i < elements.Length; i++)
                {
                    Items.Add(elements[elementIndex]);
                }
            }
            finally
            {
                noModification = false;
            }
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, TCollected item)
        {
            if (!noModification)
            {
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                    OnCollectionChanging(e);
                    Items.Insert(index, item);
                    TrySetOpposite(index, item);
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.Insert(index, item);
                    TrySetOpposite(index, item);
                }
            }
        }

        private void TrySetOpposite(int index, TCollected item)
        {
            try
            {
                noModification = true;
                SetOpposite(item, Parent);
            }
            catch
            {
                Items.RemoveAt(index);
                throw;
            }
            finally
            {
                noModification = false;
            }
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            if (!noModification)
            {
                var item = this[index];
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                    OnCollectionChanging(e);
                    Items.RemoveAt(index);
                    TryUnsetOpposite(index, item);
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.RemoveAt(index);
                    TryUnsetOpposite(index, item);
                }
            }
        }

        private void TryUnsetOpposite(int index, TCollected item)
        {
            try
            {
                noModification = true;
                SetOpposite(item, default);
            }
            catch
            {
                Items.Insert(index, item);
                throw;
            }
            finally
            {
                noModification = false;
            }
        }

        /// <inheritdoc />
        protected override void SetItem(int index, TCollected item)
        {
            if (!noModification)
            {
                var oldItem = this[index];
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index);
                    OnCollectionChanging(e);
                    Items[index] = item;
                    TryReplaceOpposite(index, item, oldItem);
                    OnCollectionChanged(e, false);
                }
                else
                {
                    Items[index] = item;
                    TryReplaceOpposite(index, item, oldItem);
                }
            }
        }

        private void TryReplaceOpposite(int index, TCollected item, TCollected oldItem)
        {
            try
            {
                noModification = true;
                SetOpposite(oldItem, default(TParent));
                SetOpposite(item, Parent);
            }
            catch
            {
                Items[index] = oldItem;
                throw;
            }
            finally
            {
                noModification = false;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Opposite OrderedSet Count={Count}]";
        }
    }
}
