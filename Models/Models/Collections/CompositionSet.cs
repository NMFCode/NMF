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
    public class CompositionSet<T> : DecoratedSet<T> where T : class, IModelElement
    {
        public IModelElement Parent { get; private set; }

        public CompositionSet(IModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override bool Add(T item)
        {
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
            return base.Add(item);
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

        public override bool Remove(T item)
        {
            if (item != null && base.Remove(item))
            {
                item.ParentChanged -= RemoveItem;
                item.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }

    public class ObservableCompositionSet<T> : ObservableSet<T> where T : class, IModelElement
    {
        public IModelElement Parent { get; private set; }

        public ObservableCompositionSet(IModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override bool Add(T item)
        {
            if (item != null)
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
            return base.Add(item);
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

        public override bool Remove(T item)
        {
            if (item != null && base.Remove(item))
            {
                item.ParentChanged -= RemoveItem;
                item.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }
}
