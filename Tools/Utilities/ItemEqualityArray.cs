using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Utilities
{
    internal class ItemEqualityArray<T> : IEquatable<ItemEqualityArray<T>>, IEnumerable<T>
    {
        private readonly T[] items;

        public ItemEqualityArray(T[] items)
        {
            this.items = items;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ItemEqualityArray<T>);
        }

        public override int GetHashCode()
        {
            return ItemEqualityComparer<T>.Instance.GetHashCode(items);
        }

        public bool Equals(ItemEqualityArray<T> other)
        {
            if (other == null) return false;
            return ItemEqualityComparer<T>.Instance.Equals(items, other.items);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
