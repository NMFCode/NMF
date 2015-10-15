using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Collections
{
    public class AssociationSet<T> : DecoratedSet<T> where T : class, IModelElement
    {
        public override bool Add(T item)
        {
            if (item != null) item.Deleted += RemoveItem;
            return base.Add(item);
        }

        public override void Clear()
        {
            foreach (var item in this)
            {
                if (item != null)
                {
                    item.Deleted -= RemoveItem;
                }
            }
            base.Clear();
        }

        public override bool Remove(T item)
        {
            if (item != null) item.Deleted -= RemoveItem;
            return base.Remove(item);
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }

    public class ObservableAssociationSet<T> : ObservableSet<T> where T : class, IModelElement
    {
        public override bool Add(T item)
        {
            if (item != null) item.Deleted += RemoveItem;
            return base.Add(item);
        }

        public override void Clear()
        {
            foreach (var item in this)
            {
                if (item != null)
                {
                    item.Deleted -= RemoveItem;
                }
            }
            base.Clear();
        }

        public override bool Remove(T item)
        {
            if (item != null) item.Deleted -= RemoveItem;
            return base.Remove(item);
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }
}
