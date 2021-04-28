using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Collections
{
    /// <summary>
    /// Denotes an ordered set implementation to store associated elements
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class AssociationOrderedSet<T> : OrderedSet<T> where T : class, IModelElement
    {
        /// <inheritdoc />
        public override bool Add(T item)
        {
            if (item != null) item.Deleted += RemoveItem;
            return base.Add(item);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override bool Remove(T item, int index)
        {
            if (item != null) item.Deleted -= RemoveItem;
            return base.Remove(item, index);
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            Remove(item);
        }
    }

    /// <summary>
    /// Denotes an observable ordered set to store associated elements
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class ObservableAssociationOrderedSet<T> : ObservableOrderedSet<T> where T : class, IModelElement
    {
        /// <inheritdoc />
        public override bool Add(T item)
        {
            if (item != null) item.Deleted += RemoveItem;
            return base.Add(item);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override bool Remove(T item, int index)
        {
            if (item != null) item.Deleted -= RemoveItem;
            return base.Remove(item, index);
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            Remove(item);
        }
    }
}
