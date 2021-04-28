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
    /// Denotes a set to store associated elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssociationSet<T> : DecoratedSet<T> where T : class, IModelElement
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

    /// <summary>
    /// Denotes an observable set to store associated elements
    /// </summary>
    /// <typeparam name="T">The type of the elements</typeparam>
    public class ObservableAssociationSet<T> : ObservableSet<T> where T : class, IModelElement
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
