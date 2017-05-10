using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Collections
{
    public class CompositionOrderedSet<T> : OrderedSet<T> where T : class, IModelElement
    {
        public ModelElement Parent { get; private set; }

        public CompositionOrderedSet(ModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void OnInsertItem(T item, int index)
        {
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
            NotifyChangedUri(index + 1, -1);
        }

        protected override void OnRemoveItem(T item, int index)
        {
            if (item != null && base.Remove(item))
            {
                item.ParentChanged -= RemoveItem;
                item.Delete();
            }
            NotifyChangedUri(index, 1);
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

        public override void Clear()
        {
            foreach (var item in this)
            {
                item.ParentChanged -= RemoveItem;
                item.Delete();
            }
            base.Clear();
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                base.Remove(item);
            }
        }
    }

    public class ObservableCompositionOrderedSet<T> : ObservableOrderedSet<T> where T : class, IModelElement
    {
        public ModelElement Parent { get; private set; }

        public ObservableCompositionOrderedSet(ModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override void Clear()
        {
            foreach (var item in this.ToArray())
            {
                item.ParentChanged -= RemoveItem;
                item.Delete();
            }
            base.Clear();
        }

        protected override void OnInsertItem(T item, int index)
        {
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
            base.OnInsertItem(item, index);
            NotifyChangedUri(index + 1, -1);
        }

        protected override void OnRemoveItem(T item, int index)
        {
            base.OnRemoveItem(item, index);
            if (item != null && item.Parent == Parent)
            {
                item.ParentChanged -= RemoveItem;
                item.Delete();
            }
            NotifyChangedUri(index, 1);
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
                            if (string.IsNullOrEmpty(baseUri.Fragment)) return;
                            Uri oldUri;
                            string newRef = ModelHelper.CreatePath(Parent.GetCompositionName(this), i + diff);
                            if (baseUri.IsAbsoluteUri)
                            {
                                var builder = new UriBuilder(baseUri);
                                builder.Fragment = baseUri.Fragment.TrimStart('#') + newRef + '/';
                                oldUri = builder.Uri;
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

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                base.Remove(item);
            }
        }
    }
}
