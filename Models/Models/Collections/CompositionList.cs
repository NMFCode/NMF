using NMF.Collections.ObjectModel;
using NMF.Expressions;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Models.Collections
{
    /// <summary>
    /// Denotes the base class for a composition list
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class CompositionList<T> : Collection<T> where T : class, IModelElement
    {
        /// <summary>
        /// Gets the parent model element
        /// </summary>
        public ModelElement Parent { get; private set; }
        private bool silent;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="parent">The parent model element</param>
        /// <exception cref="ArgumentNullException">Thrown if the parent is null</exception>
        public CompositionList(ModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
                    if (item != null && !item.IsIdentified && item is ModelElement me)
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

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            if (!silent)
            {
                silent = true;
                var oldItem = this[index];
                if (oldItem != item)
                {
                    AddParentChangedHandler(item);
                    RemoveParentChangedHandler(oldItem);
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

        private void AddParentChangedHandler(IModelElement item)
        {
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
            }
        }

        private void RemoveParentChangedHandler(IModelElement item)
        {
            if (item != null)
            {
                item.ParentChanged -= RemoveItem;
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

    /// <summary>
    /// Denotes the base class for an observable composition list
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class ObservableCompositionList<T> : ObservableList<T> where T : class, IModelElement
    {
        /// <summary>
        /// Gets the parent model element
        /// </summary>
        public ModelElement Parent { get; private set; }
        private bool silent;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="parent">The parent model element</param>
        /// <exception cref="ArgumentNullException">Thrown if parent is null</exception>
        public ObservableCompositionList(ModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        /// <inheritdoc />
        protected override void ClearItems()
        {
            if (!silent)
            {
                silent = true;
                var elements = this.ToArray();
                base.ClearItems();
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
                silent = false;
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
                    if (item != null && (!item.IsIdentified || !ModelElement.PreferIdentifiers) && item is ModelElement me)
                    {
                        UpdateUri(diff, parentUri, i, me);
                    }
                }
            }
        }

        private void UpdateUri(int diff, Lazy<Uri> parentUri, int i, ModelElement me)
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

        /// <inheritdoc />
        protected override void InsertItem(int index, T item)
        {
            if (!silent && item != null)
            {
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                    OnCollectionChanging(e);
                    Items.Insert(index, item);
                    silent = true;
                    item.ParentChanged += RemoveItem;
                    item.Parent = Parent;
                    silent = false;
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.Insert(index, item);
                    silent = true;
                    item.ParentChanged += RemoveItem;
                    item.Parent = Parent;
                    silent = false;
                }
                NotifyChangedUri(index + 1, -1);
            }
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            if (!silent)
            {
                var item = this[index];
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                    OnCollectionChanging(e);
                    Items.RemoveAt(index);
                    silent = true;
                    Delete(item);
                    silent = false;
                    OnCollectionChanged(e, true);
                }
                else
                {
                    Items.RemoveAt(index);
                    silent = true;
                    Delete(item);
                    silent = false;
                }
                NotifyChangedUri(index, 1);
            }
        }

        private void Delete(T item)
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

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            if (!silent)
            {
                var oldItem = this[index];
                if (RequireEvents())
                {
                    var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index);
                    OnCollectionChanging(e);
                    Items[index] = item;
                    BeforeSetItemPropagates(item, oldItem);
                    OnCollectionChanged(e, false);
                }
                else
                {
                    Items[index] = item;
                    BeforeSetItemPropagates(item, oldItem);
                }
            }
        }

        private void BeforeSetItemPropagates(T item, T oldItem)
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
