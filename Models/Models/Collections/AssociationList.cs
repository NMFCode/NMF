using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    /// <summary>
    /// Denotes a collection to store associated elements
    /// </summary>
    /// <typeparam name="T">The type of the elements</typeparam>
    public class AssociationList<T> : Collection<T> where T : class, IModelElement
    {
        /// <inheritdoc />
        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item != null)
                {
                    item.Deleted -= RemoveItem;
                }
            }
            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, T item)
        {
            if (item != null)
            {
                item.Deleted += RemoveItem;
            }
            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            this[index].Deleted -= RemoveItem;
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            var currentValue = this[index];
            if (currentValue != null)
            {
                currentValue.Deleted -= RemoveItem;
            }
            if (item != null)
            {
                item.Deleted += RemoveItem;
            }
            base.SetItem(index, item);
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }

    /// <summary>
    /// An observable collection to store associated model elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableAssociationList<T> : ObservableList<T> where T : class, IModelElement
    {
        /// <inheritdoc />
        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item != null)
                {
                    item.Deleted -= RemoveItem;
                }
            }
            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, T item)
        {
            if (item != null)
            {
                item.Deleted += RemoveItem;
            }
            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            this[index].Deleted -= RemoveItem;
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            var currentValue = this[index];
            if (currentValue != null)
            {
                currentValue.Deleted -= RemoveItem;
            }
            if (item != null)
            {
                item.Deleted += RemoveItem;
            }
            base.SetItem(index, item);
        }

        private void RemoveItem(object sender, EventArgs e)
        {
            var item = sender as T;
            base.Remove(item);
        }
    }
}
