using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public override bool Add(T item)
        {
            if (base.Add(item))
            {
                if (item != null)
                {
                    item.ParentChanged += RemoveItem;
                    item.Parent = Parent;
                }
                return true;
            }
            return false;
        }

        public override void Insert(int index, T item)
        {
            base.Insert(index, item);
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
            NotifyChangedUri(index + 1, -1);
        }

        protected override bool Remove(T item, int index)
        {
            if (base.Remove(item, index))
            {
                if (item != null && base.Remove(item))
                {
                    item.ParentChanged -= RemoveItem;
                    item.Delete();
                }
                NotifyChangedUri(index, 1);
                return true;
            }
            return false;
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
                        if (item is ModelElement me)
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
            var elements = this.ToArray();
            if (RequireEvents())
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                foreach (var item in elements)
                {
                    item.ParentChanged -= RemoveItem;
                    item.Delete();
                }
                OnCollectionChanged(e);
            }
            else
            {
                SilentClear();
                foreach (var item in elements)
                {
                    item.ParentChanged -= RemoveItem;
                    item.Delete();
                }
            }
        }

        public override bool Add(T item)
        {
            if (!RequireEvents())
            {
                if (SilentAdd(item))
                {
                    Register(item);
                    return true;
                }
                return false;
            }
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count);
            OnCollectionChanging(e);
            if (SilentAdd(item))
            {
                Register(item);
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        private void Register(T item)
        {
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
        }

        public override void Insert(int index, T item)
        {
            if (!RequireEvents())
            {
                SilentInsert(index, item);
                Register(item);
                NotifyChangedUri(index + 1, -1);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                OnCollectionChanging(e);
                SilentInsert(index, item);
                Register(item);
                NotifyChangedUri(index + 1, -1);
                OnCollectionChanged(e);
            }
        }

        protected override bool Remove(T item, int index)
        {
            if (!RequireEvents())
            {
                if (SilentRemove(item, index))
                {
                    Unregister(item);
                    NotifyChangedUri(index, -1);
                }
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                OnCollectionChanging(e);
                if (SilentRemove(item, index))
                {
                    Unregister(item);
                    NotifyChangedUri(index, -1);
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
            return false;
        }

        private void Unregister(T item)
        {
            if (item != null)
            {
                item.ParentChanged -= RemoveItem;
                if (item.Parent == Parent) item.Delete();
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
                        if (item is ModelElement me)
                        {
                            var baseUri = parentUri.Value;
                            if (baseUri == null) return;
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
