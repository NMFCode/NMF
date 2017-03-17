using NMF.Collections.ObjectModel;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    public class CompositionList<T> : Collection<T> where T : class, IModelElement
    {
        public ModelElement Parent { get; private set; }
        private bool silent;

        public CompositionList(ModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void ClearItems()
        {
            if (!silent)
            {
                silent = true;
                foreach (var item in this)
                {
                    if (item != null)
                    {
                        item.ParentChanged -= RemoveItem;
                        if (item.Parent == Parent)
                        {
                            item.Delete();
                        }
                    }
                }
                base.ClearItems();
                silent = false;
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (!silent && item != null)
            {
                item.ParentChanged += RemoveItem;
                silent = true;
                item.Parent = Parent;
                base.InsertItem(index, item);
                silent = false;
                NotifyChangedUri(index + 1, -1);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (!silent)
            {
                silent = true;
                var item = this[index];
                base.RemoveItem(index);
                if (item != null)
                {
                    item.ParentChanged -= RemoveItem;
                    if (item.Parent == Parent)
                    {
                        item.Delete();
                    }
                }
                silent = false;
                NotifyChangedUri(index, 1);
            }
        }

        private void NotifyChangedUri(int startIndex, int diff)
        {
            if (Parent.IsFlagSet(ModelElement.ModelElementFlag.RequireUris))
            {
                var parentUri = new Lazy<Uri>(() => Parent.AbsoluteUri ?? Parent.RelativeUri);
                for (int i = startIndex; i < Count; i++)
                {
                    var item = this[i];
                    if (item != null && !item.IsIdentified)
                    {
                        var me = item as ModelElement;
                        if (me != null)
                        {
                            var baseUri = parentUri.Value;
                            Uri oldUri;
                            var newRef = ModelHelper.CreatePath(Parent.GetCompositionName(this), i + diff);
                            if (baseUri.IsAbsoluteUri)
                            {
                                oldUri = new Uri(baseUri, baseUri.Fragment + "/" + newRef);
                            }
                            else
                            {
                                oldUri = new Uri(baseUri.OriginalString + "/" + newRef, UriKind.Relative);
                            }
                            me.OnUriChanged(oldUri);
                        }
                    }
                }
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (!silent)
            {
                silent = true;
                var oldItem = this[index];
                if (oldItem != item)
                {
                    if (item != null)
                    {
                        item.ParentChanged += RemoveItem;
                    }
                    if (oldItem != null)
                    {
                        oldItem.ParentChanged -= RemoveItem;
                    }
                    base.SetItem(index, item);
                    if (oldItem != null && oldItem.Parent == Parent)
                    {
                        oldItem.Delete();
                    }
                    if (item != null)
                    {
                        item.Parent = Parent;
                    }
                }
                silent = false;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                Remove(item);
            }
        }
    }

    public class ObservableCompositionList<T> : ObservableList<T> where T : class, IModelElement
    {
        public ModelElement Parent { get; private set; }
        private bool silent;

        public ObservableCompositionList(ModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void ClearItems()
        {
            if (!silent)
            {
                silent = true;
                var elements = this.ToArray();
                base.ClearItems();
                silent = false;
            }
        }

        protected override void BeforeClearPropagates(T[] elements)
        {
            foreach (var item in elements)
            {
                if (item != null)
                {
                    item.ParentChanged -= RemoveItem;
                    if (item.Parent == Parent)
                    {
                        item.Delete();
                    }
                }
            }
        }

        private void NotifyChangedUri(int startIndex, int diff)
        {
            if (Parent.IsFlagSet(ModelElement.ModelElementFlag.RequireUris))
            {
                var parentUri = new Lazy<Uri>(() => Parent.AbsoluteUri ?? Parent.RelativeUri);
                for (int i = startIndex; i < Count; i++)
                {
                    var item = this[i];
                    if (item != null && (!item.IsIdentified || !ModelElement.PreferIdentifiers))
                    {
                        var me = item as ModelElement;
                        if (me != null)
                        {
                            var baseUri = parentUri.Value;
                            Uri oldUri;
                            var newRef = ModelHelper.CreatePath(Parent.GetCompositionName(this), i + diff);
                            if (baseUri.IsAbsoluteUri)
                            {
                                oldUri = new Uri(baseUri, baseUri.Fragment + "/" + newRef);
                            }
                            else
                            {
                                oldUri = new Uri(baseUri.OriginalString + "/" + newRef, UriKind.Relative);
                            }
                            me.OnUriChanged(oldUri);
                        }
                    }
                }
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (!silent && item != null)
            {
                base.InsertItem(index, item);
                NotifyChangedUri(index + 1, -1);
            }
        }

        protected override void BeforeInsertPropagates(int index, T item)
        {
            silent = true;
            item.ParentChanged += RemoveItem;
            item.Parent = Parent;
            silent = false;
        }

        protected override void RemoveItem(int index)
        {
            if (!silent)
            {
                base.RemoveItem(index);
                NotifyChangedUri(index, 1);
            }
        }

        protected override void BeforeRemovePropagates(int index, T item)
        {
            silent = true;
            if (item != null)
            {
                item.ParentChanged -= RemoveItem;
                if (item.Parent == Parent)
                {
                    item.Delete();
                }
            }
            silent = false;
        }

        protected override void SetItem(int index, T item)
        {
            if (!silent)
            {
                base.SetItem(index, item);
            }
        }

        protected override void BeforeSetItemPropagates(int index, T item, T oldItem)
        {
            silent = true;
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
            }
            if (oldItem != null)
            {
                oldItem.ParentChanged -= RemoveItem;
            }

            if (oldItem != null && oldItem.Parent == Parent)
            {
                oldItem.Delete();
            }
            if (item != null)
            {
                item.Parent = Parent;
            }
            silent = true;
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            var item = sender as T;
            Remove(item);
        }
    }
}
