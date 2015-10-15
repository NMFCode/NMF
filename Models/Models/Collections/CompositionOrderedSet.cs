using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Collections
{
    public class CompositionOrderedSet<T> : OrderedSet<T> where T : class, IModelElement
    {
        public IModelElement Parent { get; private set; }

        public CompositionOrderedSet(IModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override bool Add(T item)
        {
            if (item != null)
            {
                item.Deleted += RemoveItem;
                item.Parent = Parent;
            }
            return base.Add(item);
        }

        public override void Clear()
        {
            foreach (var item in this)
            {
                item.Deleted -= RemoveItem;
                item.Parent = null;
                item.Delete();
            }
            base.Clear();
        }

        public override bool Remove(T item)
        {
            if (item != null && base.Remove(item))
            {
                item.Deleted -= RemoveItem;
                item.Parent = null;
                item.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }

    public class ObservableCompositionOrderedSet<T> : ObservableOrderedSet<T> where T : class, IModelElement
    {
        public IModelElement Parent { get; private set; }

        public ObservableCompositionOrderedSet(IModelElement parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        public override bool Add(T item)
        {
            if (item != null)
            {
                item.Deleted += RemoveItem;
                item.Parent = Parent;
            }
            return base.Add(item);
        }

        public override void Clear()
        {
            foreach (var item in this)
            {
                item.Deleted -= RemoveItem;
                item.Parent = null;
                item.Delete();
            }
            base.Clear();
        }

        public override bool Remove(T item)
        {
            if (item != null && base.Remove(item))
            {
                item.Deleted -= RemoveItem;
                item.Parent = null;
                item.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }
}
