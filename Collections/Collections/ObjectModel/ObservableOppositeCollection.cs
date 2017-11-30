using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            if (!RequireEvents())
            {
                SilentClear();
                foreach (var item in elements)
                {
                    SetOpposite(item, default(TParent));
                }
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                foreach (var item in elements)
                {
                    SetOpposite(item, default(TParent));
                }
                OnCollectionChanged(e);
            }
        }

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
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count);
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

        public override void Insert(int index, TCollected item)
        {
            if (!RequireEvents())
            {
                SilentInsert(index, item);
                SetOpposite(item, Parent);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                OnCollectionChanging(e);
                SilentInsert(index, item);
                SetOpposite(item, Parent);
                OnCollectionChanged(e);
            }
        }

        protected override bool Remove(TCollected item, int index)
        {
            if (!RequireEvents())
            {
                if(SilentRemove(item, index))
                {
                    SetOpposite(item, default(TParent));
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
                    SetOpposite(item, default(TParent));
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        protected override void Replace(int index, TCollected oldValue, TCollected newValue)
        {
            if (!RequireEvents())
            {
                SilentReplace(index, oldValue, newValue);
                SetOpposite(oldValue, default(TParent));
                SetOpposite(newValue, Parent);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                OnCollectionChanging(e);
                SilentReplace(index, oldValue, newValue);
                SetOpposite(oldValue, default(TParent));
                SetOpposite(newValue, Parent);
                OnCollectionChanged(e);
            }
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
            if (!RequireEvents())
            {
                SilentClear();
                foreach (var item in elements)
                {
                    SetOpposite(item, default(TParent));
                }
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                foreach (var item in elements)
                {
                    SetOpposite(item, default(TParent));
                }
                OnCollectionChanged(e);
            }
        }

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

        public override bool Remove(TCollected item)
        {
            if (!RequireEvents())
            {
                if (SilentRemove(item))
                {
                    SetOpposite(item, default(TParent));
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
                    SetOpposite(item, default(TParent));
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
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
                var elements = this.ToArray();
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    OnCollectionChanging(e);
                    Items.Clear();
                    noModification = true;
                    foreach (var item in elements)
                    {
                        SetOpposite(item, default(TParent));
                    }
                    noModification = false;
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.Clear();
                    noModification = true;
                    foreach (var item in elements)
                    {
                        SetOpposite(item, default(TParent));
                    }
                    noModification = false;
                }
            }
        }

        protected override void InsertItem(int index, TCollected item)
        {
            if (!noModification)
            {
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                    OnCollectionChanging(e);
                    Items.Insert(index, item);
                    noModification = true;
                    SetOpposite(item, Parent);
                    noModification = false;
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.Insert(index, item);
                    noModification = true;
                    SetOpposite(item, Parent);
                    noModification = false;
                }
            }
        }

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
                    noModification = true;
                    SetOpposite(item, default(TParent));
                    noModification = false;
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.RemoveAt(index);
                    noModification = true;
                    SetOpposite(item, default(TParent));
                    noModification = false;
                }
            }
        }

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
                    noModification = true;
                    SetOpposite(oldItem, default(TParent));
                    SetOpposite(item, Parent);
                    noModification = false;
                    OnCollectionChanged(e, false);
                }
                else
                {
                    Items[index] = item;
                    noModification = true;
                    SetOpposite(oldItem, default(TParent));
                    SetOpposite(item, Parent);
                    noModification = false;
                }
            }
        }
    }
}
