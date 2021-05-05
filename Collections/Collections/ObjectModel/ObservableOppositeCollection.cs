using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
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

        protected override void Replace(int index, TCollected oldValue, TCollected newValue)
        {
            if (!RequireEvents())
            {
                SilentReplace(index, oldValue, newValue);
                try
                {
                    SetOpposite(oldValue, default(TParent));
                    SetOpposite(newValue, Parent);
                }
                catch
                {
                    SilentReplace(index, newValue, oldValue);
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
                    SetOpposite(oldValue, default(TParent));
                    SetOpposite(newValue, Parent);
                }
                catch
                {
                    SilentReplace(index, newValue, oldValue);
                    throw;
                }
                OnCollectionChanged(e);
            }
        }

        public override string ToString()
        {
            return $"[Opposite OrderedSet Count={Count}]";
        }
    }

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

        public override string ToString()
        {
            return $"[Opposite Set Count={Count}]";
        }
    }

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
                    SetOpposite(elements[elementIndex], default(TParent));
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
                SetOpposite(item, default(TParent));
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

        public override string ToString()
        {
            return $"[Opposite OrderedSet Count={Count}]";
        }
    }
}
